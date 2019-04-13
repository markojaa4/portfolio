using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Hladnjaca
{
    class Otkup
    {
        public static List<Otkup> OtkupLista = new List<Otkup>();
        public int JedinstveniBrojDokumenta { get; private set; }
        public Voce Voce { get; set; }
        public decimal KolicinaIKlase { get; set; }
        public decimal KolicinaIIKlase { get; set; }
        public decimal KolicinaIIIKlase { get; set; }
        public double AmbalazeUzeto { get; set; }
        public double AmbalazeVraceno { get; set; }
        public Otkup(int jedinstveniBroj, Voce voce, decimal kolicinaIKlase, decimal kolicinaIIKlase, decimal kolicinaIIIKlase, double ambalazeUzeto, double ambalazeVraceno)
        {
            JedinstveniBrojDokumenta = jedinstveniBroj;
            Voce = voce;
            KolicinaIKlase = kolicinaIKlase;
            KolicinaIIKlase = kolicinaIIKlase;
            KolicinaIIIKlase = kolicinaIIIKlase;
            AmbalazeUzeto = ambalazeUzeto;
            AmbalazeVraceno = ambalazeVraceno;
        }
        public void Insert()
        {
            string connectionString = Pomocna.ConStr;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter jedinstveniBrojDokumenta = new SqlParameter("@jedinstveni_broj_dokumenta", SqlDbType.Int);
                    SqlParameter voceId = new SqlParameter("@voce_ID", SqlDbType.Int);
                    SqlParameter kolicinaIKlase = new SqlParameter("@kolicina_i_klase", SqlDbType.Decimal);
                    SqlParameter kolicinaIIKlase = new SqlParameter("@kolicina_ii_klase", SqlDbType.Decimal);
                    SqlParameter kolicinaIIIKlase = new SqlParameter("@kolicina_iiI_klase", SqlDbType.Decimal);
                    SqlParameter ambalazeUzeto = new SqlParameter("@ambalaze_uzeto", SqlDbType.Float);
                    SqlParameter ambalazeVraceno = new SqlParameter("@ambalaze_vraceno", SqlDbType.Float);
                    command.Parameters.Add(jedinstveniBrojDokumenta);
                    command.Parameters.Add(voceId);
                    command.Parameters.Add(kolicinaIKlase);
                    command.Parameters.Add(kolicinaIIKlase);
                    command.Parameters.Add(kolicinaIIIKlase);
                    command.Parameters.Add(ambalazeUzeto);
                    command.Parameters.Add(ambalazeVraceno);
                    jedinstveniBrojDokumenta.Value = JedinstveniBrojDokumenta;
                    voceId.Value = Voce.Id;
                    kolicinaIKlase.Value = KolicinaIKlase;
                    kolicinaIIKlase.Value = KolicinaIIKlase;
                    kolicinaIIIKlase.Value = KolicinaIIIKlase;
                    ambalazeUzeto.Value = AmbalazeUzeto;
                    ambalazeVraceno.Value = AmbalazeVraceno;
                    command.CommandText = "INSERT INTO otkup (jedinstveni_broj_dokumenta, voce_ID, kolicina_i_klase, kolicina_ii_klase, kolicina_iii_klase, ambalaze_uzeto, ambalaze_vraceno) VALUES (@jedinstveni_broj_dokumenta, @voce_ID, @kolicina_i_klase, @kolicina_ii_klase, @kolicina_iii_klase, @ambalaze_uzeto, @ambalaze_vraceno)";
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void Populate()
        {
            List<Otkup> otkupLista = new List<Otkup>();
            string connectionString = Pomocna.ConStr;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT jedinstveni_broj_dokumenta, kolicina_i_klase, kolicina_ii_klase, kolicina_iii_klase, ambalaze_uzeto, ambalaze_vraceno, ID, otkupno_mesto_ID FROM otkup INNER JOIN voce ON voce_ID = voce.ID";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int jedinstveniBrojDokumenta = (int)reader.GetValue(0);
                            decimal kolicinaIKlase = (decimal)reader.GetValue(1);
                            decimal kolicinaIIKlase = (decimal)reader.GetValue(2);
                            decimal kolicinaIIIKlase = (decimal)reader.GetValue(3);
                            double ambalazeUzeto = (double)reader.GetValue(4);
                            double ambalazeVraceno = (double)reader.GetValue(5);
                            int voceId = (int)reader.GetValue(6);
                            int otkupnoMestoId = (int)reader.GetValue(7);
                            Voce voce = OtkupnoMesto.OtkupnaMesta.Find(o => o.Id == otkupnoMestoId).VoceLista.Find(v => v.Id == voceId);
                            Otkup otkup = new Otkup(jedinstveniBrojDokumenta, voce, kolicinaIKlase, kolicinaIIKlase, kolicinaIIIKlase, ambalazeUzeto, ambalazeVraceno);
                            otkupLista.Add(otkup);
                        }
                    }
                }
            }
            OtkupLista = otkupLista;
        }
        public static Otkup VrsiOtkup(Proizvodjac proizvodjac, Voce voce, decimal kolicinaIKlase, decimal kolicinaIIKlase, decimal kolicinaIIIKlase, int jedinstveniBrojDokumenta, double ambalazeUzeto, double ambalazeVraceno)
        {
            Otkup duplikat = OtkupLista.Find(o => o.JedinstveniBrojDokumenta == jedinstveniBrojDokumenta);
            if (!(duplikat is null))
                throw new KorisnickiException("Takav broj dokumenta vec postoji");
            if (!voce.OtkupnoMesto.Proizvodjaci.Contains(proizvodjac))
                throw new KorisnickiException("Izabrani proizvodjac ne pripada odgovarajucem otkupnom mestu.");
            decimal cena = 0;
            cena += kolicinaIKlase * voce.CenaIKlase;
            cena += kolicinaIIKlase * voce.CenaIIKlase;
            cena += kolicinaIIIKlase * voce.CenaIIIKlase;
            Otkup otkup = new Otkup(jedinstveniBrojDokumenta, voce, kolicinaIKlase, kolicinaIIKlase, kolicinaIIIKlase, ambalazeUzeto, ambalazeVraceno);
            proizvodjac.AmbalazeZaduzeno -= ambalazeVraceno;
            proizvodjac.UnosZaduzenjaAmbalaze(ambalazeUzeto);
            proizvodjac.Saldo -= cena;
            proizvodjac.AzurirajSaldo();
            otkup.Insert();
            OtkupLista.Add(otkup);
            Pomocna.Loguj($"Otkup br. {jedinstveniBrojDokumenta}.");
            return otkup;
        }
        public static string PregledOtkupa()
        {
            string pregled = "";
            foreach (Otkup otk in OtkupLista)
            {
                pregled += otk.ToString() + "\n";
            }
            Pomocna.Loguj("Pregled otkupa.");
            return pregled;
        }
        public override string ToString()
        {
            return $"{JedinstveniBrojDokumenta}: {Voce.Naziv} ({KolicinaIKlase} prve klase, {KolicinaIIKlase} druge klase, {KolicinaIIIKlase} trece klase); {AmbalazeUzeto} ambalaze uzeto, {AmbalazeVraceno} vraceno.";
        }
    }
}
