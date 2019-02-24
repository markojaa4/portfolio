namespace FighterLib
{
    internal abstract class Fighter
    {
        public int MaxStrength { get; } = 5;
        public int MaxSpeed { get; } = 5;
        public int MaxStamina { get; } = 20;
        protected Ability action;
        public Action Act { get; protected set; }
        public int Points { get; protected set; }
        protected int strength;
        public int Strength
        {
            get => strength;
            set
            {
                strength = value;
                if (strength < 0)
                    strength = 0;
                else if (strength > MaxStrength)
                    strength = MaxStrength;
            }
        }
        protected int speed;
        public int Speed
        {
            get => speed;
            set
            {
                speed = value;
                if (speed < 0)
                    speed = 0;
                else if (speed > MaxSpeed)
                    speed = MaxSpeed;
            }
        }
        protected int stamina;
        public int Stamina
        {
            get => stamina;
            set
            {
                stamina = value;
                if (stamina < 0)
                    stamina = 0;
                else if (stamina > MaxStamina)
                    stamina = MaxStamina;
            }
        }
        protected bool Attack(IFighter opponent)
        {
            if (Strength * Speed > opponent.Stamina && opponent.Action != Ability.Defend)
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
        protected bool Recover(IFighter opponent)
        {
            if (Strength < MaxStrength || Speed < MaxSpeed || Stamina < MaxStamina)
            {
                Strength++;
                Speed++;
                Stamina++;
                return true;
            }
            return false;
        }
        public Fighter()
        {
            Points = 0;
            Strength = MaxStrength;
            Speed = MaxSpeed;
            Stamina = MaxStamina;
        }
    }
}
