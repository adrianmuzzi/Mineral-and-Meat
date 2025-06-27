using System;

namespace CustomProject;

/// <summary>
/// Represents a single tile on the game map. A tile may contain a <see cref="MapObject"/> or be empty.
/// </summary>
public class Tile {
    private MapObject? _content;

    /// <summary>
    /// Initializes a new instance of the <see cref="Tile"/> class with a specified <see cref="MapObject"/>.
    /// </summary>
    /// <param name="obj">The map object to place in the tile; can be null for an empty tile.</param>
    public Tile(MapObject? obj) {
        _content = obj;
    }

    /// <summary>
    /// Initializes a new empty tile.
    /// </summary>
    public Tile() : this(null) {}

    /// <summary>
    /// Gets or sets the map object occupying this tile.
    /// </summary>
    public MapObject? Content {
        get => _content;
        set => _content = value;
    }

    /// <summary>
    /// Determines whether the tile is empty (i.e., has no content).
    /// </summary>
    /// <returns><c>true</c> if the tile is empty - otherwise, <c>false</c>.</returns>
    public bool IsEmpty() {
        if(_content == null)
            return true;
        return false;
    }

    /// <summary>
    /// Clears the content of the tile, setting it to null.
    /// </summary>
    public void ClearContent() {
        _content = null;
    }

    /// <summary>
    /// Prints a representation of the tile to the console.
    /// If the tile is empty, prints 'X'; otherwise, invokes the <see cref="MapObject.PrintSelf"/> method.
    /// </summary>
    public void PrintTile() {
        if(_content == null) {
            Console.Write("X");
        } else {
            _content.PrintSelf();
        }
    }
}
