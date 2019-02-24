using System;
using static System.Console;
using FighterLib;

namespace FighterClient
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
                p1Caption = "COMPUTER";
                p2Caption = "YOU";
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
    
    
    
    
    
    
    
    
    
    
    
    
}

