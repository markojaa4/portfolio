namespace FighterLib
{
    /// <summary>
    /// Represents the appropriate method to use when a fighter acts.
    /// </summary>
    /// <param name="opponent">The opponent fighter instance.</param>
    /// <returns>True if the move was effective, false if not.</returns>
    public delegate bool Action(IFighter opponent);
}
