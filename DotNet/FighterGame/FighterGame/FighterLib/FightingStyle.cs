namespace FighterLib
{
    /// <summary>
    /// Specifies the fighting styles of fighters.
    /// </summary>
    public enum FightingStyle : byte
    {
        /// <summary>
        /// Corresponds to <see cref="FighterLib.Attacker"/>.
        /// </summary>
        Attacker,

        /// <summary>
        /// Corresponds to <see cref="FighterLib.Swarmer"/>.
        /// </summary>
        Swarmer,

        /// <summary>
        /// Corresponds to <see cref="FighterLib.Defender"/>.
        /// </summary>
        Defender
    }
}
