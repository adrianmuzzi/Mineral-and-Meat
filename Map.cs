using System;

namespace CustomProject;

/// <summary>
/// Represents the game map, containing a 2D grid of tiles and map-related utilities.
/// </summary>
public class Map {
    private int _width = 9;
    private int _height = 9;
    private Tile[,] _tiles;

    /// <summary>
    /// Initializes a new instance of the <see cref="Map"/> class with a predefined layout and resource tiles.
    /// </summary>
    public Map() {
        _tiles = new Tile[_width, _height];
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                _tiles[x, y] = new Tile();
            }
        }

        // Pre-populate map with resource tiles
        _tiles[3, 3].Content = new Resource(new Dictionary<ResourceType, int> { { ResourceType.Mineral, 25 }, { ResourceType.Meat, 10 } }, "Resource", "Resource");
        _tiles[2, 8].Content = new Resource(new Dictionary<ResourceType, int> { { ResourceType.Mineral, 40 }, { ResourceType.Meat, 5 } }, "Resource", "Resource");
        _tiles[1, 7].Content = new Resource(new Dictionary<ResourceType, int> { { ResourceType.Mineral, 15 }, { ResourceType.Meat, 10 } }, "Resource", "Resource");
        _tiles[7, 1].Content = new Resource(new Dictionary<ResourceType, int> { { ResourceType.Mineral, 5 }, { ResourceType.Meat, 10 } }, "Resource", "Resource");
        _tiles[8, 4].Content = new Resource(new Dictionary<ResourceType, int> { { ResourceType.Mineral, 10 }, { ResourceType.Meat, 15 } }, "Resource", "Resource");
        _tiles[5, 8].Content = new Resource(new Dictionary<ResourceType, int> { { ResourceType.Mineral, 30 }, { ResourceType.Meat, 5 } }, "Resource", "Resource");
        _tiles[4, 0].Content = new Resource(new Dictionary<ResourceType, int> { { ResourceType.Mineral, 25 }, { ResourceType.Meat, 10 } }, "Resource", "Resource");
        _tiles[3, 6].Content = new Resource(new Dictionary<ResourceType, int> { { ResourceType.Mineral, 15 }, { ResourceType.Meat, 15 } }, "Resource", "Resource");
        _tiles[6, 3].Content = new Resource(new Dictionary<ResourceType, int> { { ResourceType.Mineral, 15 }, { ResourceType.Meat, 15 } }, "Resource", "Resource");
        _tiles[1, 4].Content = new Resource(new Dictionary<ResourceType, int> { { ResourceType.Mineral, 25 }, { ResourceType.Meat, 15 } }, "Resource", "Resource");
    }

    /// <summary>
    /// Gets the width of the map.
    /// </summary>
    public int Width => _width;

    /// <summary>
    /// Gets the height of the map.
    /// </summary>
    public int Height => _height;

    /// <summary>
    /// Retrieves the tile located at the specified coordinates.
    /// </summary>
    /// <param name="x">The horizontal coordinate.</param>
    /// <param name="y">The vertical coordinate.</param>
    /// <returns>The <see cref="Tile"/> at the specified location.</returns>
    public Tile GetTile(int x, int y) {
        return _tiles[x, y];
    }

    /// <summary>
    /// Gets the coordinates of the specified <see cref="MapObject"/> on the map.
    /// </summary>
    /// <param name="obj">The map object to locate.</param>
    /// <returns>A tuple representing the (x, y) location of the object.</returns>
    /// <exception cref="Exception">Thrown if the object is not found on the map.</exception>
    public (int,int) GetMapLocation(MapObject obj) {
        for(int x = 0; x < _width; x++) {
            for(int y = 0; y < _height; y++) {
                if(_tiles[x, y].Content == obj)
                    return (x, y);
            }
        }
        throw new Exception("MapObject not found on Map");
    }

    /// <summary>
    /// Prints the map to the console. Optionally only displays content of tiles in the visibleTiles parameter.
    /// </summary>
    /// <param name="visibleTiles">If passed in, only display content information for these tiles. Rest '?'</param>
    public void PrintMap(HashSet<(int, int)>? visibleTiles = null) {
        Console.WriteLine($"--- Lay of the land ---{Environment.NewLine}");

        //print column headers
        Console.Write("   ");
        for (int w = 0; w < _width; w++) {
            Console.Write($"   {w}");
        }
        Console.WriteLine($"{Environment.NewLine}");

        //print rows with y labels
        for (int y = 0; y < _height; y++) {
            Console.Write($" {y} ");
            for(int x = 0; x < _width; x++) {
                Console.Write("   ");
                if (visibleTiles == null)   //print everything if no argument passed in
                    _tiles[x, y].PrintTile();
                else if (visibleTiles.Contains((x, y))) //if argument passed, print specific tile if visible
                    _tiles[x, y].PrintTile();
                else {  //if not visible, print ?
                    ConsoleColor originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("?");
                    Console.ForegroundColor = originalColor;
                }
            }
            Console.WriteLine($"{Environment.NewLine}");
        }
    }

    /// <summary>
    /// Determines whether the specified (x, y) coordinates are within the bounds of the map.
    /// </summary>
    /// <param name="x">The x-coordinate to check.</param>
    /// <param name="y">The y-coordinate to check.</param>
    /// <returns><c>true</c> if the coordinates are within bounds; otherwise, <c>false</c>.</returns>
    public bool InBounds(int x, int y) {
        return x >= 0 && x < _width && y >= 0 && y < _height;
    }
}