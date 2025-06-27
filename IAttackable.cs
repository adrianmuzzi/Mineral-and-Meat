using System;

namespace CustomProject;

/// <summary>
/// Represents an attackable entity within the game, such as units or structures.
/// </summary>
public interface IAttackable
{
    /// <summary>
    /// Gets the owner of the attackable object, used to manage the owner's units or structures.
    /// </summary>
    public Player Owner { get; }

    /// <summary>
    /// Gets the current hit points (HP) of the object.
    /// </summary>
    public int HP { get; }

    /// <summary>
    /// Gets the sight range, or the number of adjacent tiles visible to the object.
    /// </summary>
    public int SightRange { get; }

    /// <summary>
    /// Applies incoming damage to the object and returns its remaining HP.
    /// </summary>
    /// <param name="dmg">The amount of damage to apply.</param>
    /// <returns>The remaining HP after taking damage.</returns>
    public int TakeDamage(int dmg);
}