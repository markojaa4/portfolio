using System;
using static System.Console;

namespace FighterLib
{
    /// <summary>
    /// Represents a human-controlled player.
    /// </summary>
    public class Human : IPlayer
    {
        public IFighter Fighter { get; private set; }

        public void Play(IFighter opponent)
        {
            try
            {
                string controls = "<A> attack | <S> recover";
                if (Fighter.Style == FightingStyle.Defender)
                    controls += " | <D> defend";
                Write($"[ {controls} ] ");
                ConsoleKey key = ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.A:
                        Fighter.Action = Ability.Attack;
                        Write("<A>");
                        break;
                    case ConsoleKey.S:
                        Fighter.Action = Ability.Recover;
                        Write("<S>");
                        break;
                    case ConsoleKey.D:
                        Fighter.Action = Ability.Defend;
                        Write("<D>");
                        break;
                    default:
                        throw new UsageException("Invalid action.");
                }
                WriteLine();
            }
            catch (UsageException e)
            {
                WriteLine(e.Message);
                Play(opponent);
            }
        }

        public void Resolve(IFighter opponent)
        {
            WriteLine($"You {Fighter.Action}.");
            if (Fighter.Act(opponent))
                WriteLine("Success!");
            else
                WriteLine("Ineffective.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Human"/> class by prompting the user for needed input.
        /// </summary>
        public Human()
        {
            Fighter = PickFighter();
        }

        private static IFighter PickFighter()
        {
            WriteLine($"Choose your fighter type: " +
                      $"{FightingStyle.Attacker.ToString().ToLower()}, " +
                      $"{FightingStyle.Swarmer.ToString().ToLower()} or " +
                      $"{FightingStyle.Defender.ToString().ToLower()}.");
            string choice = ReadLine();
            choice = choice.ToLower();
            switch (choice)
            {
                case "attacker":
                    return new Attacker();
                case "swarmer":
                    return new Swarmer();
                case "defender":
                    return new Defender();
                default:
                    WriteLine("Sorry, what was that?");
                    return PickFighter();
            }
        }
    }
}
