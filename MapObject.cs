using System;

namespace CustomProject;

/// <summary>
/// Represents a base class for all objects that can exist on the map,
/// including resources, structures, and units.
/// </summary>
public abstract class MapObject {
    protected string _name;
    protected string _description;

    /// <summary>
    /// Initializes a new instance of the <see cref="MapObject"/> class.
    /// </summary>
    /// <param name="name">The display name of the map object.</param>
    /// <param name="desc">The description of the map object.</param>
    public MapObject(string name, string desc) {
        _name = name;
        _description = desc;
    }

    /// <summary>
    /// Gets the name of the map object.
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// Gets the description of the map object.
    /// </summary>
    public virtual string Description => _description;

    /// <summary>
    /// Prints the object’s representation to the console.
    /// Implementing classes define their own output logic.
    /// </summary>
    public abstract void PrintSelf();
}
