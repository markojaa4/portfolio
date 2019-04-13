using System;
using System.Data;
using System.Data.SqlClient;
using static System.Console;

namespace Hladnjaca
{
    class Program
    {
        static void Main()
        {
            try
            {
                WriteLine("Ucitavanje...");
                OtkupnoMesto.Populate();
                OdabirAkcije();
            }
            catch (Exception e)
            {
                WriteLine("Fatalna greska:\n" + e.Message + "\nProgram se zatvara.");
            }
            WriteLine("Pritisni bilo koji taster...");
            ReadKey();
        }
        //-----------------------------------------------------------------------------------------
        static void OdabirAkcije()
        {
            try
            {
                while (true)
                {
                    WriteLine("Izaberite akciju:");
                    WriteLine("0) Izlaz iz programa");
                    WriteLine("1) Unos otkupnog mesta");
                    WriteLine("2) Unos proizvodjaca");
                    WriteLine("3) Postavi otkupljivaca");
                    WriteLine("4) Unos voca");
                    WriteLine("5) Unos zaduzenja ambalaze");
                    WriteLine("6) Unos otkupa");
                    WriteLine("7) Pregled otkupa");
                    WriteLine("8) Pregled salda");
                    string izbor = ReadLine();
                    izbor = izbor.ToLower();
                    switch (izbor)
                    {
                        case "0":
                        case "izlaz":
                            return;
                        case "1":
                        case "unos otkupnog mesta":
                            WriteLine("Ime otkupnog mesta?");
                            OtkupnoMesto.UnosOtkupnogMesta(ReadLine());
                            break;
                        case "2":
                        case "unos proizvodjaca":
                            UnosProizvodjaca();
                            break;
                        case "3":
                        case "postavi otkupljivaca":
                            PostaviOtkupljivaca();
                            break;
                        case "4":
                        case "unos voca":
                            UnosVoca();
                            break;
                        case "5":
                        case "unos zaduzenja ambalaze":
                            UnosZaduzenjaAmbalaze();
                            break;
                        case "6":
                        case "unos otkupa":
                            UnosOtkupa();
                            break;
                        case "7":
                        case "pregled otkupa":
                            WriteLine(Otkup.PregledOtkupa());
                            break;
                        case "8":
                        case "pregled salda":
                            WriteLine(CitajOtkupnoMesto().PregledSalda());
                            break;
                        default:
                            WriteLine("Nepostojeca akcija.");
                            break;
                    }
                }
            }
            catch (KorisnickiException e)
            {
                WriteLine(e);
                OdabirAkcije();
            }
            catch (SqlException)
            {
                WriteLine("Proveri ispravnost podataka i pokusaj ponovo.");
                OdabirAkcije();
            }
            catch (DataException)
            {
                WriteLine("Proveri ispravnost podataka i pokusaj ponovo.");
                OdabirAkcije();
            }
        }
        //-----------------------------------------------------------------------------------------
        static void UnosProizvodjaca()
        {
            OtkupnoMesto otkupnoMesto = CitajOtkupnoMesto();
            WriteLine("Ime?");
            string ime = ReadLine();
            WriteLine("Prezime?");
            string prezime = ReadLine();
            WriteLine("JMBG?");
            string jmbg = ReadLine();
            WriteLine("Telefon?");
            string telefon = ReadLine();
            WriteLine("Broj tekuceg racuna?");
            string racun = ReadLine();
            WriteLine("Povrsina parcele?");
            CitajBroj(out double povrsinaParcele);
            otkupnoMesto.UnosProizvodjaca(ime, prezime, jmbg, telefon, racun, povrsinaParcele);
        }
        //-----------------------------------------------------------------------------------------
        static void PostaviOtkupljivaca()
        {
            OtkupnoMesto otkupnoMesto = CitajOtkupnoMesto();
            Proizvodjac proizvodjac = CitajProizvodjaca(otkupnoMesto);
            otkupnoMesto.PostaviOtkupljivaca(proizvodjac);
        }
        //-----------------------------------------------------------------------------------------
        static void UnosVoca()
        {
            OtkupnoMesto otkupnoMesto = CitajOtkupnoMesto();
            WriteLine("Uneti naziv voca.");
            string naziv = ReadLine();
            WriteLine("Unesite cenu za prvu klasu.");
            CitajBroj(out decimal cenaI);
            WriteLine("Unesite cenu za drugu klasu.");
            CitajBroj(out decimal cenaII);
            WriteLine("Unesite cenu za trecu klasu.");
            CitajBroj(out decimal cenaIII);
            otkupnoMesto.UnosVoca(naziv, cenaI, cenaII, cenaIII);
        }
        //-----------------------------------------------------------------------------------------
        static void UnosZaduzenjaAmbalaze()
        {
            OtkupnoMesto otkupnoMesto = CitajOtkupnoMesto();
            Proizvodjac proizvodjac = CitajProizvodjaca(otkupnoMesto);
            WriteLine("Unesi kolicinu ambalaze za koju se proizvodjac zaduzuje.");
            CitajBroj(out double kolicina);
            proizvodjac.UnosZaduzenjaAmbalaze(kolicina);
        }
        //-----------------------------------------------------------------------------------------
        static void UnosOtkupa()
        {
            OtkupnoMesto otkupnoMesto = CitajOtkupnoMesto();
            Proizvodjac proizvodjac = CitajProizvodjaca(otkupnoMesto);
            Voce voce = CitajVoce(otkupnoMesto);
            WriteLine("Unesi kolicinu I klase.");
            CitajBroj(out decimal kolicinaI);
            WriteLine("Unesi kolicinu II klase.");
            CitajBroj(out decimal kolicinaII);
            WriteLine("Unesi kolicinu III klase.");
            CitajBroj(out decimal kolicinaIII);
            WriteLine("Unesi koliko ambalaze se zaduzuje.");
            CitajBroj(out double ambZaduzeno);
            WriteLine("Unesi koliko ambalaze je vraceno.");
            CitajBroj(out double ambVraceno);
            WriteLine("Unesi jedinstveni broj dokumenta.");
            CitajBroj(out int jedinstvBrDok);
            Otkup.VrsiOtkup(proizvodjac, voce, kolicinaI, kolicinaII, kolicinaIII, jedinstvBrDok, ambZaduzeno, ambVraceno);
        }
        //-----------------------------------------------------------------------------------------
        static OtkupnoMesto CitajOtkupnoMesto()
        {
            WriteLine("Izberi otkupno mesto.");
            for (int i = 0; i < OtkupnoMesto.OtkupnaMesta.Count; i++)
                WriteLine($"{i + 1}) { OtkupnoMesto.OtkupnaMesta[i].Ime}");
            CitajBroj(out int izbor);
            if (izbor < 0 || izbor > OtkupnoMesto.OtkupnaMesta.Count)
                throw new KorisnickiException("Neispravan unos.");
            OtkupnoMesto otkupnoMesto = OtkupnoMesto.OtkupnaMesta[izbor - 1];
            return otkupnoMesto;
        }
        //-----------------------------------------------------------------------------------------
        static Proizvodjac CitajProizvodjaca(OtkupnoMesto otkupnoMesto)
        {
            WriteLine("Izaberi proizvodjaca.");
            for (int i = 0; i < otkupnoMesto.Proizvodjaci.Count; i++)
                WriteLine($"{i + 1}) {otkupnoMesto.Proizvodjaci[i].Ime} {otkupnoMesto.Proizvodjaci[i].Prezime} {otkupnoMesto.Proizvodjaci[i].JMBG}");
            CitajBroj(out int izbor);
            if (izbor < 0 || izbor > otkupnoMesto.Proizvodjaci.Count)
                throw new KorisnickiException("Neispravan unos.");
            Proizvodjac proizvodjac = otkupnoMesto.Proizvodjaci[izbor - 1];
            return proizvodjac;
        }
        //-----------------------------------------------------------------------------------------
        static Voce CitajVoce(OtkupnoMesto otkupnoMesto)
        {
            WriteLine("Izaberi voce.");
            for (int i = 0; i < otkupnoMesto.VoceLista.Count; i++)
                WriteLine($"{i + 1}) {otkupnoMesto.VoceLista[i].Naziv}");
            CitajBroj(out int izbor);
            if (izbor < 0 || izbor > otkupnoMesto.VoceLista.Count)
                throw new KorisnickiException("Neispravan unos.");
            Voce voce = otkupnoMesto.VoceLista[izbor - 1];
            return voce;
        }
        //-----------------------------------------------------------------------------------------
        static void CitajBroj(out decimal br)
        {
            bool prolazi;
            while (true)
            {
                prolazi = decimal.TryParse(ReadLine(), out br);
                if (prolazi == false)
                    WriteLine("Neispravan unos. Unesite broj.");
                else
                    return;
            }
        }
        //-----------------------------------------------------------------------------------------
        static void CitajBroj(out double br)
        {
            bool prolazi;
            while (true)
            {
                prolazi = double.TryParse(ReadLine(), out br);
                if (prolazi == false)
                    WriteLine("Neispravan unos. Unesite broj.");
                else
                    return;
            }
        }
        //------------------------------------------------------------------------------------------
        static void CitajBroj(out int br)
        {
            bool prolazi;
            while (true)
            {
                try
                {
                    checked
                    {
                        prolazi = int.TryParse(ReadLine(), out br);
                    }
                    if (prolazi == false)
                        WriteLine("Neispravan unos. Unesite broj.");
                    else
                        return;
                }
                catch (OverflowException)
                {
                    throw new KorisnickiException("Broj izlazi iz opsega dozvoljenih vrednosti.");
                }
            }
        }
    }
}
