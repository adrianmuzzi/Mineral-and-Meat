using System;

namespace CustomProject;

/// <summary>
/// Represents the Vigilant unit type. Inherits from <see cref="Unit"/>.
/// </summary>
public class Vigilant : Unit {
    /// <summary>
    /// Gets the static resource cost to produce a Vigilant.
    /// Accessible publicly for affordability calculations by <see cref="Player"/>.
    /// </summary>
    public static readonly Dictionary<ResourceType, int> _cost = new Dictionary<ResourceType, int> {
        { ResourceType.Mineral, 10 },
        { ResourceType.Meat, 10 }
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="Vigilant"/> unit with predefined stats.
    /// </summary>
    /// <param name="owner">The player that owns this unit.</param>
    public Vigilant(Player owner) : base("Vigilant", "A weary watcher", owner, 15, 3, 5) { }

    /// <summary>
    /// Prints the unit's character representation in the owning player's color.
    /// </summary>
    public override void PrintSelf() {
        ConsoleColor originalColor = Console.ForegroundColor;
        Console.ForegroundColor = _owner.Color;
        Console.Write("V");
        Console.ForegroundColor = originalColor;
    }
}
