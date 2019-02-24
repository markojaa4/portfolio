namespace FighterLib
{
    internal class Attacker : Fighter, IFighter
    {
        public FightingStyle Style { get; } = FightingStyle.Attacker;
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
            if (Strength * Speed > opponent.Stamina && opponent.Action != Ability.Defend)
            {
                Points++;
                Strength--;
                opponent.Stamina -= 2;
                return true;
            }
            Strength -= 2;
            opponent.Strength--;
            return false;
        }
        public Attacker() : base()
        {
            Action = Ability.Attack;
        }
    }
}
