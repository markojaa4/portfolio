namespace FighterLib
{
    /// <summary>
    /// Represents a player that controls a fighter through input.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// The fighter that the player is controlling.
        /// </summary>
        IFighter Fighter { get; }

        /// <summary>
        /// Gets input for preparing an action and sets the fighter's prepared action.
        /// </summary>
        /// <param name="opponent">The fighter that the action is targeting.</param>
        void Play(IFighter opponent);

        /// <summary>
        /// Plays out the prepared action.
        /// </summary>
        /// <param name="opponent">The fighter that the move is targeting.</param>
        void Resolve(IFighter opponent);
    }
}
