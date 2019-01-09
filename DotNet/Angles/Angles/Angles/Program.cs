using System;
using static System.Console;

namespace GetSetEtc
{
    static class Consts
    {
        public const string InputSymbol = "> ";
        public const string Separator = "______________\n";
    }
    class Program
    {
        class Angle
        {
            int degree;
            int minute;
            int second;
            public Angle(int d, int m, int s)
            {
                ValidifyAngle(d, m, s);
                degree = d;
                minute = m;
                second = s;
            }
            public int Second
            {
                get
                {
                    return second;
                }
                set
                {
                    ValidifyAngle(Degree, Minute, value);
                    second = value;
                }
            }
            public int Minute
            {
                get
                {
                    return minute;
                }
                set
                {
                    ValidifyAngle(Degree, value, Second);
                    minute = value;
                }
            }
            public int Degree
            {
                get
                {
                    return degree;
                }
                set
                {
                    ValidifyAngle(value, Minute, Second);
                    degree = value;
                }
            }
            private void ValidifyAngle(int d, int m, int s)
            {
                while (s > 59)
                {
                    m++;
                    s -= 60;
                }
                while (m > 59)
                {
                    d++;
                    m -= 60;
                }
                while (s < 0)
                {
                    s += 60;
                    m--;
                }
                while (m < 0)
                {
                    m += 60;
                    d--;
                }
                if (d < 0) throw new Exception("negative angle");
                if
                (
                    d > 360 ||
                    (
                        d >= 360 &&
                        (m > 0 || s > 0)
                    )
                ) throw new Exception("angle over 360 degrees");
            }
            public static Angle operator +(Angle alpha, Angle beta)
            {
                Angle re = new Angle(0, 0, 0);
                re.Second += alpha.Second + beta.Second;
                re.Minute += alpha.Minute + beta.Minute;
                re.Degree += alpha.Degree + beta.Degree;
                return re;
            }
            public static Angle operator -(Angle alpha, Angle beta)
            {
                Angle re = new Angle(0, 0, 0);
                re.Degree += alpha.Degree - beta.Degree;
                re.Minute += alpha.Minute - beta.Minute;
                re.Second += alpha.Second - beta.Second;
                return re;
            }
            public static Angle operator ++(Angle alpha)
            {
                Angle i = new Angle(0, 0, 1);
                return alpha + i;
            }
            public static Angle operator --(Angle alpha)
            {
                Angle i = new Angle(0, 0, 1);
                return alpha - i;
            }
            public static bool operator ==(Angle alpha, Angle beta)
            {
                if (alpha.Degree == beta.Degree &&
                    alpha.Minute == beta.Minute &&
                    alpha.Second == beta.Second)
                    return true;
                else return false;
            }
            public override bool Equals(object obj)
            {
                Angle AngleObj = (Angle)obj;
                if (Degree == AngleObj.Degree ||
                    Minute == AngleObj.Minute ||
                    Second == AngleObj.Second)
                    return true;
                else return false;
            }
            public override int GetHashCode()
            {
                return Second + Minute + Degree;
            }
            public static bool operator !=(Angle alpha, Angle beta)
            {
                if (alpha.degree != beta.degree ||
                    alpha.minute != beta.minute ||
                    alpha.second != beta.second)
                    return true;
                else return false;
            }
            public override string ToString() => $"{degree} degrees,\n{minute} minutes,\n{second} seconds";
        }
        //====================================================================================
        class AngleOperator
        {
            public static readonly string[] opts = { "++", "--", "+", "-", "==", "!=" };
            string symbol;
            public string Symbol
            {
                get
                {
                    if (symbol == null) throw new Exception("operator stores no input");
                    return symbol;
                }
            }
            public static string ListOpts()
            {
                string optString = "";
                for (int i = 0; i < opts.Length; i++)
                {
                    optString += opts[i];
                    if (i == opts.Length - 2)
                        optString += " or ";
                    else if (i != opts.Length - 1)
                        optString += ", ";
                }
                return optString;
            }
            public void ReadIn()
            {
                bool isValid = false;
                bool isFirst = true;
                do
                {
                    if (!isFirst)
                    {
                        WriteErr("invalid value, enter " + ListOpts() + '.');
                        Write(Consts.InputSymbol);
                    }
                    isFirst = false;
                    symbol = ReadLine();
                    foreach (string opt in opts)
                    {
                        if (symbol == opt) isValid = true;
                    }
                } while (!isValid);
            }
            public static bool operator ==(AngleOperator oper, string comp)
            {
                if (oper.Symbol == comp) return true;
                else return false;
            }
            public override bool Equals(object obj)
            {
                if (Symbol == (string)obj) return true;
                else return false;
            }
            public override int GetHashCode()
            {
                return 0;
            }
            public static bool operator !=(AngleOperator oper, string comp)
            {
                if (oper.Symbol != comp) return true;
                else return false;
            }
        }
        //====================================================================================
        static void Main(string[] args)
        {
            try
            {
                Work();
            }
            catch (Exception e)
            {
                WriteErr("error: " + e.Message);
                WriteLine("Ending program...");
            }
            WriteLine("\n\nPress any key to exit.");
            ReadKey();
        }
        //====================================================================================
        static void WriteErr(string msg)
        {
            BackgroundColor = ConsoleColor.Red;
            ForegroundColor = ConsoleColor.White;
            WriteLine(msg);
            ResetColor();
        }
        //====================================================================================
        static void GetVal(out int val)
        {
            bool isValid = false;
            bool isFirst = true;
            do
            {
                if (!isFirst)
                {
                    WriteErr("invalid value, enter an integer:");
                    Write(Consts.InputSymbol);
                }
                isFirst = false;
                isValid = int.TryParse(ReadLine(), out val);
            } while (!isValid);
        }
        //====================================================================================
        static Angle GetAngle()
        {
            try
            {
                Write("Degrees " + Consts.InputSymbol);
                GetVal(out int degrees);
                Write("Minutes " + Consts.InputSymbol);
                GetVal(out int minutes);
                Write("Seconds " + Consts.InputSymbol);
                GetVal(out int seconds);
                return new Angle(degrees, minutes, seconds);
            }
            catch (Exception e)
            {
                WriteErr(e.Message);
                WriteLine("Enter different values.");
                return GetAngle();
            }
        }
        //====================================================================================
        static AngleOperator GetOperator()
        {
            WriteLine("Specify operator (" + AngleOperator.ListOpts() + "):");
            Write(Consts.InputSymbol);
            AngleOperator oper = new AngleOperator();
            oper.ReadIn();
            return oper;
        }
        //====================================================================================
        static void Work()
        {
            try
            {
                const string AngleA = "Alpha";
                const string AngleB = "Beta";
                WriteLine($"Specify angle {AngleA} (0-360 degrees):");
                Angle alpha = GetAngle();
                AngleOperator oper = GetOperator();
                while (oper == "++" || oper == "--")
                {
                    if (oper == "++") alpha++;
                    else alpha--;
                    WriteLine(Consts.Separator);
                    WriteLine(AngleA + " = ");
                    WriteLine(alpha);
                    WriteLine(Consts.Separator);
                    oper = GetOperator();
                }
                WriteLine($"Specify angle {AngleB} (0-360 degrees):");
                Angle beta = GetAngle();
                WriteLine(Consts.Separator);
                Write($"The result of {AngleA} {oper.Symbol} {AngleB} is: ");
                if (oper == "+") alpha += beta;
                else if (oper == "-") alpha -= beta;
                if (oper == "==") WriteLine(alpha == beta);
                else if (oper == "!=") WriteLine(alpha != beta);
                else WriteLine("\n" + alpha);
                WriteLine(Consts.Separator);
            }
            catch (Exception e)
            {
                WriteErr(e.Message);
                WriteLine("Starting over...");
                Work();
            }
        }
    }
}
