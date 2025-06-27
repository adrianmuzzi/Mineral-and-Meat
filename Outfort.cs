using System;

namespace CustomProject;

/// <summary>
/// Represents the Outfort structure type. Inherits from <see cref="Structure"/>.
/// Outforts are used to create Vigilants.
/// </summary>
public class Outfort : Structure {
    /// <summary>
    /// Gets the static construction cost of an Outfort.
    /// Accessible without needing to instantiate the object.
    /// </summary>
    public static readonly Dictionary<ResourceType, int> _cost = new Dictionary<ResourceType, int> {
        { ResourceType.Mineral, 40 },
        { ResourceType.Meat, 10 }
    };

    /// <summary>
    /// Initializes a new instance of the class with preset attributes.
    /// </summary>
    /// <param name="owner">The player that owns this structure.</param>
    public Outfort(Player owner) : base("Outfort", "Beleaguered yet stoic. Trains Vigilants.", owner, 35, 3) {}

    /// <summary>
    /// Gets the resource cost to produce a unit from this structure.
    /// </summary>
    public override Dictionary<ResourceType, int> ProductionCost => Vigilant._cost;

    /// <summary>
    /// Produces a new <see cref="Vigilant"/> unit for the owning player.
    /// </summary>
    /// <returns>A new instance of <see cref="Vigilant"/>.</returns>
    public override Unit Produce()
    {
        return new Vigilant(_owner);
    }

    /// <summary>
    /// Prints the structure's character representation in the owning player's color.
    /// </summary>
    public override void PrintSelf() {
        ConsoleColor originalColor = Console.ForegroundColor;
        Console.ForegroundColor = _owner.Color;
        Console.Write("O");
        Console.ForegroundColor = originalColor;
    }
}