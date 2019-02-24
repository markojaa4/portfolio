using System;
using static System.Console;

namespace FighterLib
{
    /// <summary>
    /// Represents a computer-controlled player.
    /// </summary>
    public class Computer : IPlayer
    {
        public IFighter Fighter { get; private set; }

        internal delegate void PlayStyle(IFighter opponent);

        private PlayStyle playStyle;

        private void PlayAttacker(IFighter opponent)
        {
            if (Fighter.Strength == 0 || Fighter.Speed == 0)
            {
                Fighter.Action = Ability.Recover;
                return;
            }
            int rnd = Rnd.Next(1, 4);
            switch (rnd)
            {
                case 1:
                case 2:
                    if (Fighter.Strength * Fighter.Speed <= opponent.Stamina)
                        Fighter.Action = Ability.Recover;
                    else
                        Fighter.Action = Ability.Attack;
                    break;
                case 3:
                    if (Fighter.Strength < Fighter.MaxStrength ||
                        Fighter.Speed < Fighter.MaxSpeed ||
                        Fighter.Stamina < Fighter.MaxStamina)
                        Fighter.Action = Ability.Attack;
                    else
                        Fighter.Action = Ability.Recover;
                    break;
                default:
                    throw new Exception("Computer opponent failure.");
            }
        }

        private void PlaySwarmer(IFighter opponent)
        {
            if (Fighter.Strength == 0 || Fighter.Speed == 0)
            {
                Fighter.Action = Ability.Recover;
                return;
            }
            int rnd = Rnd.Next(1, 5);
            switch (rnd)
            {
                case 1:
                case 2:
                case 3:
                    if (Fighter.Strength * Fighter.Speed * 2 <= opponent.Stamina)
                        Fighter.Action = Ability.Recover;
                    else
                        Fighter.Action = Ability.Attack;
                    break;
                case 4:
                    if (Fighter.Strength < Fighter.MaxStrength ||
                        Fighter.Speed < Fighter.MaxSpeed ||
                        Fighter.Stamina < Fighter.MaxStamina)
                        Fighter.Action = Ability.Attack;
                    else
                        Fighter.Action = Ability.Recover;
                    break;
                default:
                    throw new Exception("Computer opponent failure.");
            }
        }

        private void PlayDefender(IFighter opponent)
        {
            if (Fighter.Strength == 0 || Fighter.Speed == 0)
            {
                Fighter.Action = Ability.Recover;
                return;
            }
            int rnd = Rnd.Next(1, 6);
            switch (rnd)
            {
                case 1:
                    if (Fighter.Strength * Fighter.Speed <= opponent.Stamina)
                        goto case 2;
                    else
                        Fighter.Action = Ability.Attack;
                    break;
                case 2:
                case 3:
                case 4:
                    if (Fighter.Speed < 2 || Fighter.Strength == 0)
                        goto case 5;
                    else
                        Fighter.Action = Ability.Defend;
                    break;
                case 5:
                    if (Fighter.Strength < Fighter.MaxStrength ||
                        Fighter.Speed < Fighter.MaxSpeed ||
                        Fighter.Stamina < Fighter.MaxStamina)
                        Fighter.Action = Ability.Attack;
                    else
                        Fighter.Action = Ability.Recover;
                    break;
                default:
                    throw new Exception("Computer opponent failure.");
            }
        }

        public void Play(IFighter opponent)
        {
            playStyle(opponent);
        }

        public void Resolve(IFighter opponent)
        {
            WriteLine($"Computer {Fighter.Action.ToString().ToLower()}s.");
            if (Fighter.Act(opponent))
                WriteLine("Successful!");
            else
                WriteLine("Ineffective.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Computer"/> class through random fighter selection.
        /// </summary>
        public Computer()
        {
            int rnd = Rnd.Next(1, 4);
            switch (rnd)
            {
                case 1:
                    Fighter = new Attacker();
                    playStyle = PlayAttacker;
                    break;
                case 2:
                    Fighter = new Swarmer();
                    playStyle = PlaySwarmer;
                    break;
                case 3:
                    Fighter = new Defender();
                    playStyle = PlayDefender;
                    break;
                default:
                    throw new Exception("Failed to generate computer oponent.");
            }
            WriteLine($"Computer has chosen {Fighter.Style.ToString().ToLower()}.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Computer"/> class using a specified style.
        /// </summary>
        /// <param name="style">The type of fighter that the computer will play.</param>
        public Computer(FightingStyle style)
        {
            switch (style)
            {
                case FightingStyle.Attacker:
                    Fighter = new Attacker();
                    playStyle = PlayAttacker;
                    break;
                case FightingStyle.Swarmer:
                    Fighter = new Swarmer();
                    playStyle = PlaySwarmer;
                    break;
                case FightingStyle.Defender:
                    Fighter = new Defender();
                    playStyle = PlayDefender;
                    break;
                default:
                    throw new ArgumentException("Invalid style.");
            }
            WriteLine($"Computer is {Fighter.Style.ToString().ToLower()}.");
        }
    }
}
