using System;

namespace CustomProject;

/// <summary>
/// Represents the Crier unit type. Inherits from <see cref="Unit"/>.
/// </summary>
public class Crier : Unit {
    /// <summary>
    /// Gets the static resource cost to produce a Crier.
    /// Accessible publicly for affordability calculations by <see cref="Player"/>.
    /// </summary>
    public static readonly Dictionary<ResourceType, int> _cost = new Dictionary<ResourceType, int> {
        { ResourceType.Mineral, 5 },
        { ResourceType.Meat, 15 }
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="Crier"/> unit with predefined stats.
    /// </summary>
    /// <param name="owner">The player that owns this unit.</param>
    public Crier(Player owner) : base("Crier", "A mournful prophet", owner, 10, 2, 10) { }

    /// <summary>
    /// Prints the unit's character representation in the owning player's color.
    /// </summary>
    public override void PrintSelf() {
        ConsoleColor originalColor = Console.ForegroundColor;
        Console.ForegroundColor = _owner.Color;
        Console.Write("C");
        Console.ForegroundColor = originalColor;
    }
}
