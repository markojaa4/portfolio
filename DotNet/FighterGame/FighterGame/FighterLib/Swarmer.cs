namespace FighterLib
{
    internal class Swarmer : Fighter, IFighter
    {
        public FightingStyle Style { get; } = FightingStyle.Swarmer;
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
                    default:
                        throw new UsageException("Invalid action.");
                }
                action = value;
            }
        }
        protected new bool Attack(IFighter opponent)
        {
            if (Strength * Speed * 2 > opponent.Stamina && opponent.Action != Ability.Defend)
            {
                Points++;
                Strength--;
                opponent.Stamina--;
                return true;
            }
            Strength -= 2;
            opponent.Strength--;
            return false;
        }
        public Swarmer() : base()
        {
            Action = Ability.Attack;
        }
    }
}
