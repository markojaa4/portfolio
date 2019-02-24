namespace FighterLib
{
    internal class Defender : Fighter, IFighter
    {
        public FightingStyle Style { get; } = FightingStyle.Defender;
        public Ability Action
        {
            get => action;
            set
            {
                switch (value)
                {
                    case Ability.Attack:
                        Act = Attack;
                        break;
                    case Ability.Recover:
                        Act = Recover;
                        break;
                    case Ability.Defend:
                        if (Strength == 0)
                            throw new UsageException("Not enough strength to defend.");
                        if (Speed < 2)
                            throw new UsageException("Not enough speed to defend.");
                        Act = Defend;
                        break;
                    default:
                        throw new UsageException("Invalid action.");
                }
                action = value;
            }
        }
        protected bool Defend(IFighter opponent)
        {
            Speed -= 2;
            if (opponent.Action == Ability.Attack)
            {
                Points++;
                opponent.Stamina -= 3;
                return true;
            }
            return false;
        }
        public Defender() : base()
        {
            Action = Ability.Attack;
        }
    }
}
