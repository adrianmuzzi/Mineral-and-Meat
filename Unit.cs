using System;

namespace CustomProject;

/// <summary>
/// Represents an abstract base class for all units in the game.
/// Units are map objects that can take damage and attack other attackable targets.
/// </summary>
public abstract class Unit : MapObject, IAttackable {
    protected Player _owner;
    private int _hp;
    private int _sightRange;
    private int _power;

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    /// <param name="name">The name of the unit.</param>
    /// <param name="desc">The description of the unit.</param>
    /// <param name="owner">The player who owns the unit.</param>
    /// <param name="hp">The health points of the unit.</param>
    /// <param name="sightRange">The sight range of the unit.</param>
    /// <param name="pwr">The attack power of the unit.</param>
    public Unit(string name, string desc, Player owner, int hp, int sightRange, int pwr) : base(name, desc)
    {
        _owner = owner;
        _hp = hp;
        _sightRange = sightRange;
        _power = pwr;
    }

    /// <summary>
    /// Gets the unit's description, including power and health.
    /// </summary>
    public override string Description => $"{_name} - {_description} | Power: {_power} | HP : {_hp}";

    /// <summary>
    /// Gets the owner of the unit.
    /// </summary>
    public Player Owner => _owner;

    /// <summary>
    /// Gets the current health points of the unit.
    /// </summary>
    public int HP => _hp;

    /// <summary>
    /// Gets the sight range of the unit.
    /// </summary>
    public int SightRange => _sightRange;

    /// <summary>
    /// Reduces the unit's health by a specified damage amount.
    /// </summary>
    /// <param name="dmg">The amount of damage taken.</param>
    /// <returns>The remaining health points after taking damage.</returns>
    public virtual int TakeDamage(int dmg) {
        return _hp -= dmg;
    }

    /// <summary>
    /// Attacks a target and deals damage equal to this unit's power.
    /// </summary>
    /// <param name="target">The target to attack.</param>
    /// <returns>The target's remaining health after taking damage.</returns>
    public virtual int Attack(IAttackable target) {
        return target.TakeDamage(_power);
    }
}
