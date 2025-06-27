namespace CustomProject;

/// <summary>
/// Represents the current phase of a player's turn in the game.
/// </summary>
public enum GamePhase
{
    /// <summary>
    /// The phase in which the player gathers resources from visible resource tiles.
    /// </summary>
    Harvest,

    /// <summary>
    /// The phase in which the player constructs new structures or units.
    /// </summary>
    Construct,

    /// <summary>
    /// The phase in which the player moves units and initiates attacks.
    /// </summary>
    MoveAndAttack
}
