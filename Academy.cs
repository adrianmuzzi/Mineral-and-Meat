using System;

namespace CustomProject;

/// <summary>
/// Represents the Academy structure type. Inherits from <see cref="Structure"/>.
/// Academies are used to create Criers.
/// </summary>
public class Academy : Structure {
    /// <summary>
    /// Gets the static construction cost of an Academy.
    /// Accessible without needing to instantiate the object.
    /// </summary>
    public static readonly Dictionary<ResourceType, int> _cost = new Dictionary<ResourceType, int> {
        { ResourceType.Mineral, 30 },
        { ResourceType.Meat, 15 }
    };

    /// <summary>
    /// Initializes a new instance of the class with preset attributes.
    /// </summary>
    /// <param name="owner">The player that owns this structure.</param>
    public Academy(Player owner) : base("Academy", "Harbinger factory. Trains Criers.", owner, 25, 2) {}

    /// <summary>
    /// Gets the resource cost to produce a unit from this structure.
    /// </summary>
    public override Dictionary<ResourceType, int> ProductionCost => Crier._cost;

    /// <summary>
    /// Produces a new <see cref="Crier"/> unit for the owning player.
    /// </summary>
    /// <returns>A new instance of <see cref="Crier"/>.</returns>
    public override Unit Produce()
    {
        return new Crier(_owner);
    }

    /// <summary>
    /// Prints the structure's character representation in the owning player's color.
    /// </summary>
    public override void PrintSelf() {
        ConsoleColor originalColor = Console.ForegroundColor;
        Console.ForegroundColor = _owner.Color;
        Console.Write("A");
        Console.ForegroundColor = originalColor;
    }
}