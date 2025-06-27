using System;
using System.Linq;

namespace CustomProject;

/// <summary>
/// Represents the AI-controlled player in the game.
/// </summary>
public class AIPlayer : Player {
    /// <summary>
    /// Shared random generator for AI actions.
    /// </summary>
    private static Random _rand = new Random();

    /// <summary>
    /// Indicates this player is not human.
    /// </summary>
    protected override bool IsHuman => false;

    /// <summary>
    /// Initializes an AI player starting in the bottom-right of the map.
    /// </summary>
    public AIPlayer(Map map) : base("Opponent", map, ConsoleColor.Red, map.Width - 1, map.Height - 1) { }

    /// <summary>
    /// Creates the AI's starting structures and units on the map.
    /// </summary>
    protected override void CreateStartingMapObjects(int x, int y) {
        Outfort startingOutfort = new Outfort(this);
        _map.GetTile(x, y).Content = startingOutfort;
        _structures.Add(startingOutfort);
        Vigilant startingVigilant = new Vigilant(this);
        _map.GetTile(x - 1, y - 1).Content = startingVigilant;
        _units.Add(startingVigilant);
    }

    /// <summary>
    /// Executes the AI's actions for the current game phase.
    /// </summary>
    public void TakeTurn(GamePhase phase) {
        switch (phase) {
            case GamePhase.Harvest:
                AIHarvest();
                break;
            case GamePhase.Construct:
                AIConstruct();
                break;
            case GamePhase.MoveAndAttack:
                AIMoveAndAttack();
                break;
        }
    }

    /// <summary>
    /// Executes automated harvesting logic for the AI.
    /// </summary>
    private void AIHarvest() {
        CalculateAllVisibleTiles();

        //form new set, tiles with resources only from visible tiles
        List<(int, int)> resourceTiles = _visibleTiles
                .Where(tile => _map.GetTile(tile.Item1, tile.Item2).Content is Resource)
                .ToList();

        if (resourceTiles.Count == 0)
            return;

        //harvest _random resource
        (int x, int y) harvestTile = resourceTiles[_rand.Next(resourceTiles.Count)];
        HarvestResource(harvestTile.x, harvestTile.y);
    }

    /// <summary>
    /// Executes automated construction logic for the AI, prioritizing structures first.
    /// </summary>
    private void AIConstruct() {
        CalculateAllVisibleTiles();

        //prioritise structure construction. generate list of affordable structures
        List<string> affordableStructures = new List<string>();
        if (Outfort._cost.All(kvp => _resources[kvp.Key] >= kvp.Value))
            affordableStructures.Add("outfort");
        if (Academy._cost.All(kvp => _resources[kvp.Key] >= kvp.Value))
            affordableStructures.Add("academy");

        if (affordableStructures.Count > 0) {
            //form new set with all empty tiles from visible tiles
            List<(int, int)> emptyTiles = _visibleTiles
                .Where(tile => _map.GetTile(tile.Item1, tile.Item2).IsEmpty())
                .ToList();

            if (emptyTiles.Count > 0) {
                //construct _random affordable structure on random empty tile
                (int x, int y) constructTile = emptyTiles[_rand.Next(emptyTiles.Count)];
                string structure = affordableStructures[_rand.Next(affordableStructures.Count)];
                ConstructStructure(constructTile.x, constructTile.y, structure);
                return;
            }
        }

        //if no affordable structures, build a unit
        List<int> affordableProducers = new List<int>();

        for (int i = 0; i < _structures.Count; i++) {
            //find structures with affordable production
            if (_structures[i].ProductionCost.All(kvp => _resources[kvp.Key] >= kvp.Value)) {
                affordableProducers.Add(i);
            }
        }

        if (affordableProducers.Count == 0) //cannot afford any units
            return;

        //produce from affordable structure at _random
        int builderIndex = affordableProducers[_rand.Next(affordableProducers.Count)];
        ConstructUnit(builderIndex);
    }

    /// <summary>
    /// Executes automated movement and attack logic for the AI.
    /// </summary>
    private void AIMoveAndAttack() {
        //create a shuffled list of unit indices
        List<int> unitIndices = Enumerable.Range(0, _units.Count) 
                                                .OrderBy(i => _rand.Next())
                                                .ToList();

        foreach (int i in unitIndices) {
            //create shuffled list of possible directions - right, left, down, up
            List<(int, int)> directions = new List<(int dx, int dy)> { (1, 0), (-1, 0), (0, 1), (0, -1) };
            directions = directions.OrderBy(i => _rand.Next()).ToList();

            foreach ((int, int) dir in directions) {
                if (MoveUnit(i, dir))   //return on successful move
                    return;
            }
        }
    }
}
