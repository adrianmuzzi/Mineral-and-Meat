using System;

namespace CustomProject;

public class Player {
    /// <summary>
    /// The player's name.
    /// </summary>
    protected string _name;

    /// <summary>
    /// The console color used to represent the player's assets.
    /// </summary>
    protected ConsoleColor _color;

    /// <summary>
    /// Stores the player's current resources.
    /// </summary>
    protected Dictionary<ResourceType, int> _resources = new Dictionary<ResourceType, int> { 
        { ResourceType.Mineral, 10 },
        { ResourceType.Meat, 10 }
    };

    /// <summary>
    /// The set of tiles currently visible to the player.
    /// </summary>
    protected HashSet<(int, int)> _visibleTiles = new HashSet<(int, int)>();

    /// <summary>
    /// The list of structures owned by the player.
    /// </summary>
    protected List<Structure> _structures = new List<Structure>();

    /// <summary>
    /// The list of units owned by the player.
    /// </summary>
    protected List<Unit> _units = new List<Unit>();

    /// <summary>
    /// Indicates if the player is human-controlled.
    /// </summary>
    protected virtual bool IsHuman => true;

    /// <summary>
    /// Reference to the game map.
    /// </summary>
    protected Map _map;

    /// <summary>
    /// Initializes a new player with custom position and color.
    /// </summary>
    public Player(string name, Map map, ConsoleColor color, int startX, int startY) {
        _name = name;
        _map = map;
        _color = color;
        CreateStartingMapObjects(startX, startY);
    }

    /// <summary>
    /// Initializes a new player with default position and color.
    /// </summary>
    public Player(string name, Map map) : this(name, map, ConsoleColor.Green, 0, 0) {}

    /// <summary>
    /// Creates the player's initial units and structures.
    /// </summary>
    protected virtual void CreateStartingMapObjects(int x, int y) {
        Outfort startingOutfort = new Outfort(this);
        _map.GetTile(x, y).Content = startingOutfort;
        _structures.Add(startingOutfort);
        Vigilant startingCrier = new Vigilant(this);
        _map.GetTile(x + 1, y + 1).Content = startingCrier;
        _units.Add(startingCrier);
    }

    public string Name => _name;
    public ConsoleColor Color => _color;
    public int UnitCount => _units.Count();
    public int StructureCount => _structures.Count;

    /// <summary>
    /// Gets the current set of visible tiles, recalculating visibility first.
    /// </summary>
    public HashSet<(int, int)> VisibleTiles {
        get {
            CalculateAllVisibleTiles();
            return _visibleTiles;
        }
    }

    /// <summary>
    /// Displays the map from the player's perspective.
    /// </summary>
    public void PrintPlayerMapView() {
        CalculateAllVisibleTiles();
        _map.PrintMap(_visibleTiles);
    }

    /// <summary>
    /// Updates the set of tiles visible to the player.
    /// </summary>
    protected void CalculateAllVisibleTiles() {
        _visibleTiles.Clear();
        foreach(Structure s in _structures) {
            AddVisibleTilesAroundObject(s);
        }
        foreach(Unit u in _units) {
            AddVisibleTilesAroundObject(u);
        }
    }

    /// <summary>
    /// Adds tiles within sight range of a given object to the visible set.
    /// <param name="obj">Object to calculate visibility for</param>
    /// </summary>
    private void AddVisibleTilesAroundObject(IAttackable obj) {
        (int x, int y) position = _map.GetMapLocation((MapObject) obj);
        int posX = position.Item1;
        int posY = position.Item2;

        //iterate over all tiles in the visible range from the x,y position
        //use sightrange to determine the area of tiles to iterate across
        for(int dx = -obj.SightRange; dx <= obj.SightRange; dx++) {
            for(int dy = -obj.SightRange; dy <= obj.SightRange; dy++) {
                int x = posX + dx;
                int y = posY + dy;

                if (Math.Sqrt(dx * dx + dy * dy) <= obj.SightRange) {
                    //validate that position is within map bounds and add to visible tiles if so
                    if (_map.InBounds(x, y)) {
                        _visibleTiles.Add((x, y));
                    }
                }
            }
        }
    }

    /// <summary>
    /// Attempts to harvest a resource from the specified tile.
    /// </summary>
    /// <param name="x">X coordinate for harvesting</param>
    /// <param name="y">Y coordinate for harvesting</param>
    /// <returns>True if harvest is successful</returns>
    public bool HarvestResource(int x, int y) {
        if(!_visibleTiles.Contains((x, y))) {
            Console.WriteLine("You cannot harvest from the unknown abyss");
            return false;
        }

        Tile targetTile = _map.GetTile(x, y);
        if(targetTile.Content is Resource r) {
            foreach(KeyValuePair<ResourceType, int> kvp in r.Content) {
                _resources[kvp.Key] += kvp.Value;
            }
            targetTile.ClearContent();

            if (IsHuman) {
                Console.WriteLine();
                string harvested = string.Join(" and ", r.Content.Select(kvp => $"{kvp.Value} {kvp.Key}"));
                Console.WriteLine($"Harvested {harvested}{Environment.NewLine}");
            }

            return true;
        } else {
            Console.WriteLine("Hmph. That is not a resource.");
            return false;
        }
    }

    /// <summary>
    /// Prints the player's current resources to the console.
    /// </summary>
    public void PrintResources() {
        Console.WriteLine("Your resources:");
        string output = string.Join(" | ", _resources.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        Console.WriteLine(output);
    }

    /// <summary>
    /// Attempts to construct a structure at the specified coordinates.
    /// </summary>
    /// <param name="x">X coordinate for construction</param>
    /// <param name="y">Y coordinate for construction</param>
    /// <param name="structureName">Structure to be constructed</param>
    /// <returns>True if construction is successful</returns>
    public bool ConstructStructure(int x, int y, string structureName) {
        if(structureName == "outfort") {
            if(Outfort._cost.Any(kvp => _resources[kvp.Key] < kvp.Value)) {
                Console.WriteLine("Brick and tar come at cost. You cannot afford this.");
                return false;
            }
            foreach(KeyValuePair<ResourceType, int> kvp in Outfort._cost) {
                _resources[kvp.Key] -= kvp.Value;
            }
            Structure s = new Outfort(this);
            _map.GetTile(x, y).Content = s;
            _structures.Add(s);
            return true;
        } else if(structureName == "academy") {
            if(Academy._cost.Any(kvp => _resources[kvp.Key] < kvp.Value)) {
                Console.WriteLine("Brick and tar come at cost. You cannot afford this.");
                return false;
            }
            foreach(KeyValuePair<ResourceType, int> kvp in Academy._cost) {
                _resources[kvp.Key] -= kvp.Value;
            }
            Structure s = new Academy(this);
            _map.GetTile(x, y).Content = s;
            _structures.Add(s);
            return true;
        }

        throw new Exception("structureName input invalid");
    }

    /// <summary>
    /// Attempts to construct a unit from the specified structure.
    /// </summary>
    /// <param name="structureIndex">Index in _structures to call .Produce() from</param>
    /// <returns>True if construction is successful</returns>
    public bool ConstructUnit(int structureIndex) {
        Dictionary<ResourceType, int> cost = _structures[structureIndex].ProductionCost;

        if(cost.Any(kvp => _resources[kvp.Key] < kvp.Value)) {
            Console.WriteLine("These dark halls extract a toll that you cannot meet.");
            return false;
        }

        Unit u = _structures[structureIndex].Produce();
        (int x, int y) structureLoc = _map.GetMapLocation(_structures[structureIndex]);

        (int x, int y) rightTile = (structureLoc.x + 1, structureLoc.y);
        (int x, int y) leftTile = (structureLoc.x - 1, structureLoc.y);
        (int x, int y) upTile = (structureLoc.x, structureLoc.y - 1);
        (int x, int y) downTile = (structureLoc.x, structureLoc.y + 1);

        if(_map.InBounds(rightTile.x, rightTile.y) && _map.GetTile(rightTile.x, rightTile.y).IsEmpty()) {
            _map.GetTile(rightTile.x, rightTile.y).Content = u;
        } else if(_map.InBounds(downTile.x, downTile.y) && _map.GetTile(downTile.x, downTile.y).IsEmpty()) {
            _map.GetTile(downTile.x, downTile.y).Content = u;
        } else if(_map.InBounds(leftTile.x, leftTile.y) && _map.GetTile(leftTile.x, leftTile.y).IsEmpty()) {
            _map.GetTile(leftTile.x, leftTile.y).Content = u;
        } else if(_map.InBounds(upTile.x, upTile.y) && _map.GetTile(upTile.x, upTile.y).IsEmpty()) {
            _map.GetTile(upTile.x, upTile.y).Content = u;
        } else {
            Console.WriteLine("That dark edifice has no available tile to spew its horror upon the world.");
            return false;
        }

        foreach(KeyValuePair<ResourceType, int> kvp in cost) {
            _resources[kvp.Key] -= kvp.Value;
        }

        _units.Add(u);
        if (IsHuman) {
            Console.WriteLine();
            Console.WriteLine($"A new terror unleashed. What deeds will be wrought?");
        }

        return true;
    }

    /// <summary>
    /// Prints the player's structures to the console.
    /// </summary>
    public void PrintStructures() {
        Console.WriteLine("Your Structures:");
        for(int i = 0; i < _structures.Count; i++) {
            Structure u = _structures[i];
            (int x, int y) loc = _map.GetMapLocation(_structures[i]);
            Console.WriteLine($"{i+1}. {_structures[i].Description} | Position: X: {loc.Item1}, Y: {loc.Item2}");
        }
    }

    /// <summary>
    /// Prints the resource cost of all constructible units and structures.
    /// </summary>
    public void PrintCosts() {
        string outfortCost = string.Join(" | ", Outfort._cost.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        string academyCost = string.Join(" | ", Academy._cost.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        string vigilantCost = string.Join(" | ", Vigilant._cost.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        string crierCost = string.Join(" | ", Crier._cost.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        Console.WriteLine("-- Structure Costs --");
        Console.WriteLine($"Outfort: {outfortCost}");
        Console.WriteLine($"Academy: {academyCost}");
        Console.WriteLine("-- Unit Costs --");
        Console.WriteLine($"Vigilant: {vigilantCost}");
        Console.WriteLine($"Crier: {crierCost}");
    }

    /// <summary>
    /// Attempts to move a unit in a specified direction.
    /// </summary>
    /// <param name="unitIndex">Index in _units to move</param>
    /// <param name="direction">x,y direction for movement</param>
    /// <returns>True if movement is successful</returns>
    public bool MoveUnit(int unitIndex, (int dx, int dy) direction) {
        (int x, int y) currentLoc = _map.GetMapLocation(_units[unitIndex]);
        (int x, int y) destLoc = (currentLoc.Item1 + direction.Item1, currentLoc.Item2 + direction.Item2);

        if (!_map.InBounds(destLoc.Item1, destLoc.Item2)) //validate destination inbounds
            return false;

        Tile destTile = _map.GetTile(destLoc.Item1, destLoc.Item2); //get tile
        if (destTile.IsEmpty()) { //if tile is clear, move the unit and clear original tile location
            destTile.Content = _units[unitIndex];
            _map.GetTile(currentLoc.Item1, currentLoc.Item2).ClearContent();
            if(IsHuman)
                Console.WriteLine("Move successful");
            return true;
        }

        if (destTile.Content is Resource) {
            return false; //cannot step onto a resource
        }

        //collided with a unit or structure
        IAttackable enemy = (IAttackable)destTile.Content;
        if (enemy.Owner == this) {
            return false;
        }

        //moving into an attackable unit/structure
        if (_units[unitIndex].Attack(enemy) <= 0) {
            enemy.Owner.RemoveOwnedMapObject(enemy);
            destTile.ClearContent();
            destTile.Content = _units[unitIndex];
            _map.GetTile(currentLoc.Item1, currentLoc.Item2).ClearContent();
            Console.WriteLine($"Fury and sweat overcomes as often as guile. Opposition annihilated.");
        } else {
            MapObject e = (MapObject)enemy;
            Console.WriteLine($"Struggle is inevitable. Your opposition stands with {enemy.HP} HP remaining.{Environment.NewLine}");
        }

        return true;
    }

    /// <summary>
    /// Removes a destroyed unit or structure from the player's ownership.
    /// <param name="obj">Object to remove from _units or _structures</param>
    /// </summary>
    public void RemoveOwnedMapObject(IAttackable obj) {
        switch (obj) {
            case Unit u:
                _units.Remove(u);
                break;
            case Structure s:
                _structures.Remove(s);
                break;
        }
    }

    /// <summary>
    /// Prints the player's units to the console.
    /// </summary>
    public void PrintUnits() {
        Console.WriteLine("Your Units:");
        for(int i = 0; i < _units.Count; i++) {
            Unit u = _units[i];
            (int x, int y) loc = _map.GetMapLocation(_units[i]);
            Console.WriteLine($"{i+1}. {_units[i].Description} | Position: X: {loc.Item1}, Y: {loc.Item2}");
        }
    }
}