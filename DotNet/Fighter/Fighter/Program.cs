using System;
using static System.Console;

namespace Fighter
{
    class Program
    {
        static void Main()
        {
            try
            {
                WriteLine("Welcome!");
                WriteLine();
                WriteLine("Pick a fighting style and beat up your opponent. Attacks succeed if your strength times " +
                          "speed is greater than your opponent's stamina. A successful attack costs 1 strength and " +
                          "reduces opponent's stamina by 1. A failed attack costs 2 strength and reduces opponent's " +
                          "strength by 1. Recovering increases all physical stats by 1.");
                WriteLine();
                WriteLine("An \"attacker\" style attack " +
                          "does double damage if successful. The " +
                          "\"swarmer\" style attack takes speed multiplied by 2 to determine success. The \"defender\" " +
                          "can also defend. Defending costs 2 speed, and you can't defend if you don't have enough " +
                          "speed or if your strength is 0. Defending is successful only if the opponent attacks. " +
                          "Successful defence reduces opponent's stamina by 3 and makes the attack unsuccessful.");
                WriteLine();
                WriteLine("Playes chose actions each turn. They don't know about the other player's choice until " +
                          "the turn is resolved - they have to predict the best move. Player One's move is always " +
                          "the first to take affect. However, before the fight begins, Player " +
                          "Two picks a fighting style after Player One's choice is revealed.");
                WriteLine();
                WriteLine("You win a point by successfully attacking or defending. Note that only the \"defender\" can " +
                          "defend.");
                WriteLine("___________________________________________________");
                WriteLine();
                Work();
            }
            catch (Exception e)
            {
                WriteLine("Oops, there was a fatal exception:\n" + e.Message);
                WriteLine("Ending application...");
            }
            WriteLine("Press any key to exit...");
            ReadKey();
        }
        //=============================================================================================================
        static void Work()
        {
            try
            {
                PickOrder(out IPlayer p1, out IPlayer p2);
                const int winAt = 10;
                WriteLine($"The first fighter to get {winAt} points, wins.");
                WriteLine("Begin!");
                DisplayStatus(p1, p2);
                while (true)
                {
                    p1.Play(p2.Fighter);
                    p2.Play(p1.Fighter);
                    p1.Resolve(p2.Fighter);
                    p2.Resolve(p1.Fighter);
                    DisplayStatus(p1, p2);
                    if (p1.Fighter.Points >= winAt)
                    {
                        DeclareWinner(p1);
                        break;
                    }
                    if (p2.Fighter.Points >= winAt)
                    {
                        DeclareWinner(p2);
                        break;
                    }
                }
                WriteLine("Well played.");
            }
            catch (UsageException e)
            {
                WriteLine("Error:\n" + e.Message);
                WriteLine("Let's try again.");
                Work();
            }
        }
        //=============================================================================================================
        static void PickOrder(out IPlayer playerOne, out IPlayer playerTwo)
        {
            try
            {
                WriteLine("Would you like to be Player One? (y/n)");
                string choice = ReadLine();
                if (!(choice.Length > 0))
                    throw new UsageException("Sorry, what was that?");
                choice = choice.ToLower();
                if (choice[0] == 'y')
                {
                    playerOne = new Human();
                    playerTwo = new Computer();
                }
                else if (choice[0] == 'n')
                {
                    playerOne = new Computer();
                    playerTwo = new Human();
                }
                else
                {
                    throw new UsageException("Sorry, what was that?");
                }
            }
            catch (UsageException e)
            {
                WriteLine(e.Message);
                PickOrder(out playerOne, out playerTwo);
            }
        }
        //=============================================================================================================
        static void DisplayStatus(IPlayer p1, IPlayer p2)
        {
            string p1Caption = "";
            string p2Caption = "";
            IFighter f1 = p1.Fighter;
            IFighter f2 = p2.Fighter;
            if (p1 is Human)
            {
                p1Caption = "You";
                p2Caption = "Computer";
            }
            else
            {
                p1Caption = "Computer";
                p2Caption = "You";
            }
            WriteLine("--------------------------------------");
            WriteLine($"{p1Caption}: {f1.Style}|{f1.Strength} strength|{f1.Speed} speed|{f1.Stamina} stamina|{f1.Points} points|");
            WriteLine($"{p2Caption}: {f2.Style}|{f2.Strength} strength|{f2.Speed} speed|{f2.Stamina} stamina|{f2.Points} points|");
            WriteLine("--------------------------------------");
        }
        //=============================================================================================================
        static void DeclareWinner(IPlayer winner)
        {
            if (winner is Human)
                WriteLine("You win!");
            else
                WriteLine("You lose!");
        }
    }
    class UsageException : Exception
    {
        public UsageException() : base() { }
        public UsageException(string message) : base(message) { }
        public UsageException(string message, Exception inner) : base(message, inner) { }
    }
    static class Rnd
    {
        public static Random rnd = new Random();
        public static int Next() => rnd.Next();
        public static int Next(int maxValue) => rnd.Next(maxValue);
        public static int Next(int minValue, int maxValue) => rnd.Next(minValue, maxValue);
    }
    interface IPlayer
    {
        IFighter Fighter { get; }
        void Play(IFighter opponent);
        void Resolve(IFighter opponent);
    }
    class Human : IPlayer
    {
        public IFighter Fighter { get; private set; }
        public void Play(IFighter opponent)
        {
            try
            {
                string controls = "<A> attack | <S> recover";
                if (Fighter.Style == "defender")
                    controls += " | <D> defend";
                Write($"[ {controls} ] ");
                ConsoleKey key = ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.A:
                        Write("<A>");
                        Fighter.SetAction(Ability.Attack);
                        break;
                    case ConsoleKey.S:
                        Write("<S>");
                        Fighter.SetAction(Ability.Recover);
                        break;
                    case ConsoleKey.D:
                        Write("<D>");
                        Fighter.SetAction(Ability.Defend);
                        break;
                    default:
                        WriteLine();
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
        public Human()
        {
            Fighter = PickFighter();
        }
        static Fighter PickFighter()
        {
            WriteLine("Choose your fighter type: attacker, swarmer or defender.");
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
    class Computer : IPlayer
    {
        delegate void PlayStyle(IFighter opponent);
        public IFighter Fighter { get; private set; }
        private PlayStyle playStyle;
        private void PlayAttacker(IFighter opponent)
        {
            if (Fighter.Strength == 0 || Fighter.Speed == 0)
            {
                Fighter.SetAction(Ability.Recover);
                return;
            }
            int rnd = Rnd.Next(1, 4);
            switch (rnd)
            {
                case 1:
                case 2:
                    if (Fighter.Strength * Fighter.Speed <= opponent.Stamina)
                        goto case 3;
                    else
                        Fighter.SetAction(Ability.Attack);
                    break;
                case 3:
                    Fighter.SetAction(Ability.Recover);
                    break;
                default:
                    throw new Exception("Computer opponent failure.");
            }
        }
        private void PlaySwarmer(IFighter opponent)
        {
            if (Fighter.Strength == 0 || Fighter.Speed == 0)
            {
                Fighter.SetAction(Ability.Recover);
                return;
            }
            int rnd = Rnd.Next(1, 5);
            switch (rnd)
            {
                case 1:
                case 2:
                case 3:
                    if (Fighter.Strength * Fighter.Speed * 2 <= opponent.Stamina)
                        goto case 4;
                    else
                        Fighter.SetAction(Ability.Attack);
                    break;
                case 4:
                    Fighter.SetAction(Ability.Recover);
                    break;
                default:
                    throw new Exception("Computer opponent failure.");
            }
        }
        private void PlayDefender(IFighter opponent)
        {
            if (Fighter.Strength == 0 || Fighter.Speed == 0)
            {
                Fighter.SetAction(Ability.Recover);
                return;
            }
            int rnd = Rnd.Next(1, 6);
            switch (rnd)
            {
                case 1:
                    if (Fighter.Strength * Fighter.Speed <= opponent.Stamina)
                        goto case 2;
                    else
                        Fighter.SetAction(Ability.Attack);
                    break;
                case 2:
                case 3:
                case 4:
                    if (Fighter.Speed < 2)
                        goto case 5;
                    else
                        Fighter.SetAction(Ability.Defend);
                    break;
                case 5:
                    Fighter.SetAction(Ability.Recover);
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
            WriteLine($"Computer has chosen {Fighter.Style}.");
        }
    }
    enum Ability : byte
    {
        Attack,
        Recover,
        Defend
    }
    delegate bool Action(IFighter opponent);
    interface IFighter
    {
        string Style { get; }
        int Points { get; }
        Ability Action { get; }
        int Strength { get; set; }
        int Speed { get; set; }
        int Stamina { get; set; }
        Action Act { get; }
        void SetAction(Ability action);
    }
    abstract class Fighter : IFighter
    {
        public string Style { get; } = "fighter";
        protected static readonly int maxStrength = 5;
        protected static readonly int maxSpeed = 5;
        protected static readonly int maxStamina = 20;
        public Ability Action { get; protected set; }
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
                else if (strength > maxStrength)
                    strength = maxStrength;
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
                else if (speed > maxSpeed)
                    speed = maxSpeed;
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
                else if (stamina > maxStamina)
                    stamina = maxStamina;
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
            Strength++;
            Speed++;
            Stamina++;
            return true;
        }
        public void SetAction(Ability action)
        {
            switch (action)
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
            Action = action;
        }
        public Fighter()
        {
            Points = 0;
            Strength = maxStrength;
            Speed = maxSpeed;
            Stamina = maxStamina;
        }
    }
    class Attacker : Fighter, IFighter
    {
        public new string Style { get; } = "attacker";
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
        public new void SetAction(Ability action)
        {
            switch (action)
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
            Action = action;
        }
    }
    class Swarmer : Fighter, IFighter
    {
        public new string Style { get; } = "swarmer";
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
        public new void SetAction(Ability action)
        {
            switch (action)
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
            Action = action;
        }
    }
    class Defender : Fighter, IFighter
    {
        public new string Style { get; } = "defender";
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
        public new void SetAction(Ability action)
        {
            switch (action)
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
            Action = action;
        }
    }
}
