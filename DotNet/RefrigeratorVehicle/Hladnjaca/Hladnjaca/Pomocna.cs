using System;
using System.IO;

namespace Hladnjaca
{
    class Pomocna
    {
        public static readonly string ConStr = @"Server=(localdb)\mssqllocaldb;Database=hladnjaca;Trusted_Connection=True;MultipleActiveResultSets=True";
        public static void Loguj(string akcija)
        {
            DateTime sada = DateTime.Now;
            string datum = sada.ToShortDateString();
            string vreme = sada.ToShortTimeString();
            string upis = datum + " " + vreme + " " + akcija;
            string fajl = Directory.GetCurrentDirectory() + @"/logfajl.log";
            File.AppendAllText(fajl, upis + '\n');
        }
    }
}
