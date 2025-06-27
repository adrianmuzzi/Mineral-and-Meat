using System;

namespace CustomProject;

/// <summary>
/// Represents an abstract base class for all structures in the game.
/// Structures are map objects, can produce units, and are attackable.
/// </summary>
public abstract class Structure : MapObject, IAttackable {
    protected Player _owner;
    private int _hp;
    private int _sightRange;

    /// <summary>
    /// Initializes a new instance of the <see cref="Structure"/> class.
    /// </summary>
    /// <param name="name">The name of the structure.</param>
    /// <param name="desc">The description of the structure.</param>
    /// <param name="owner">The player who owns the structure.</param>
    /// <param name="hp">The starting health points of the structure.</param>
    /// <param name="sightRange">The sight range of the structure.</param>
    public Structure(string name, string desc, Player owner, int hp, int sightRange) : base (name, desc) {
        _owner = owner;
        _hp = hp;
        _sightRange = sightRange;
    }

    /// <summary>
    /// Gets the full description of the structure including name, description and HP.
    /// </summary>
    public override string Description => $"{_name} - {_description} | HP : {_hp}";

    /// <summary>
    /// Gets the resource cost to produce a unit from this structure.
    /// </summary>
    public abstract Dictionary<ResourceType, int> ProductionCost { get; }

    /// <summary>
    /// Gets the player that owns the structure.
    /// </summary>
    public Player Owner => _owner;

    /// <summary>
    /// Gets the current health points of the structure.
    /// </summary>
    public int HP => _hp;

    /// <summary>
    /// Gets the sight range of the structure.
    /// </summary>
    public int SightRange => _sightRange;

    /// <summary>
    /// Produces a unit instance based on the structure's functionality.
    /// </summary>
    /// <returns>A new <see cref="Unit"/> produced by the structure.</returns>
    public abstract Unit Produce();

    /// <summary>
    /// Applies damage to the structure and returns remaining HP.
    /// </summary>
    /// <param name="dmg">Amount of damage dealt.</param>
    /// <returns>Remaining health points.</returns>
    public virtual int TakeDamage(int dmg) {
        return _hp -= dmg;
    }
}
