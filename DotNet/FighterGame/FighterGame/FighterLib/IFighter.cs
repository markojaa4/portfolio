namespace FighterLib
{
    /// <summary>
    /// Represents a fighter that can be controlled.
    /// </summary>
    public interface IFighter
    {
        /// <summary>
        /// Represents the type of fighter.
        /// </summary>
        FightingStyle Style { get; }

        /// <summary>
        /// The current number of points
        /// </summary>
        int Points { get; }

        /// <summary>
        /// The currently prepared action.
        /// </summary>
        Ability Action { get; set; }

        /// <summary>
        /// Current strength.
        /// </summary>
        int Strength { get; set; }

        /// <summary>
        /// Current speed.
        /// </summary>
        int Speed { get; set; }

        /// <summary>
        /// Current stamina.
        /// </summary>
        int Stamina { get; set; }

        /// <summary>
        /// Maximum attainable strength.
        /// </summary>
        int MaxStrength { get; }

        /// <summary>
        /// Maximum attainable speed.
        /// </summary>
        int MaxSpeed { get; }

        /// <summary>
        /// Maximum attainable stamina.
        /// </summary>
        int MaxStamina { get; }

        /// <summary>
        /// The method to that executes the currently prepared action.
        /// </summary>
        Action Act { get; }
    }
}
