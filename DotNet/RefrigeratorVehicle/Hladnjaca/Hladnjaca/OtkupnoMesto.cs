using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Hladnjaca
{
    class OtkupnoMesto
    {
        public int Id { get; private set; }
        public static List<OtkupnoMesto> OtkupnaMesta = new List<OtkupnoMesto>();
        public string Ime { get; private set; }
        public List<Voce> VoceLista { get; set; }
        public Proizvodjac Otkupljivac { get; set; }
        public List<Proizvodjac> Proizvodjaci { get; set; }
        public OtkupnoMesto(string ime, Proizvodjac otkupljivac = null)
        {
            Ime = ime;
            VoceLista = new List<Voce>();
            Proizvodjaci = new List<Proizvodjac>();
            Otkupljivac = otkupljivac;
        }
        public static OtkupnoMesto UnosOtkupnogMesta(string imeOtkupnogMesta)
        {
            OtkupnoMesto otkupnoMesto = new OtkupnoMesto(imeOtkupnogMesta);
            otkupnoMesto.Insert();
            OtkupnaMesta.Add(otkupnoMesto);
            Pomocna.Loguj($"Unos otkupnog mesta {imeOtkupnogMesta}");
            return otkupnoMesto;
        }
        public void PostaviOtkupljivaca(Proizvodjac otkupljivac)
        {
            if (!Proizvodjaci.Contains(otkupljivac))
                throw new KorisnickiException("Izabrani proizvodjac ne odgovara otkupnom mestu.");
            Otkupljivac = otkupljivac;
            Update();
        }
        public Proizvodjac UnosProizvodjaca(string ime, string prezime, string jmbg,
                                            string telefon, string tekuciRacun,
                                            double povrsinaParcele)
        {
            Proizvodjac proizvodjac = new Proizvodjac(ime, prezime, this)
            {
                JMBG = jmbg,
                Telefon = telefon,
                Racun = tekuciRacun,
                PovrsinaParcele = povrsinaParcele,
                AmbalazeZaduzeno = 0
            };
            proizvodjac.Insert();
            Proizvodjaci.Add(proizvodjac);
            Pomocna.Loguj($"Unos proizvodjaca: {ime} {prezime}.");
            return proizvodjac;
        }
        public void UnosVoca(string naziv, decimal cenaIKlase, decimal cenaIIKlase, decimal cenaIIIKlase)
        {
            Voce postojece = VoceLista.Find(v => v.Naziv == naziv);
            if (postojece is null)
            {
                Voce voce = new Voce(naziv, this, cenaIKlase, cenaIIKlase, cenaIIIKlase);
                voce.Insert();
                VoceLista.Add(voce);
            }
            else
            {
                postojece.CenaIKlase = cenaIKlase;
                postojece.CenaIIKlase = cenaIIKlase;
                postojece.CenaIIIKlase = cenaIIIKlase;
                postojece.Update();
            }
            Pomocna.Loguj($"Unos voca: {naziv} za {Ime}.");
        }
        public string PregledSalda()
        {
            string pregled = "";
            decimal ukupno = 0;
            foreach (Proizvodjac proizvodjac in Proizvodjaci)
            {
                pregled += $"{proizvodjac.Ime} {proizvodjac.Prezime}, {proizvodjac.OtkupnoMesto.Ime}: {proizvodjac.Saldo}.\n";
                ukupno += proizvodjac.Saldo;
            }
            pregled += $"Ukupno: {ukupno}";
            Pomocna.Loguj($"Pregled salda za mesto {Ime} (ukupno {ukupno}).");
            return pregled;
        }
        private void PopuniProizvodjace(int? idOtkupljivaca)
        {
            List<Proizvodjac> proizvodjaci = new List<Proizvodjac>();
            string connectionString = Pomocna.ConStr;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT ID, ime, prezime, jmbg, telefon, racun, povrsina_parcele, ambalaze_zaduzeno, saldo FROM proizvodjac WHERE otkupno_mesto_ID = {Id}";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader.GetValue(0);
                            string ime = (string)reader.GetValue(1);
                            string prezime = (string)reader.GetValue(2);
                            string jmbg = (string)reader.GetValue(3);
                            string telefon = (string)reader.GetValue(4);
                            string racun = (string)reader.GetValue(5);
                            double povrsinaParcele = (double)reader.GetValue(6);
                            double ambalazeZaduzeno = (double)reader.GetValue(7);
                            decimal saldo = (decimal)reader.GetValue(8);
                            Proizvodjac proizvodjac = new Proizvodjac(ime, prezime, this)
                            {
                                Id = id,
                                JMBG = jmbg,
                                Telefon = telefon,
                                Racun = racun,
                                PovrsinaParcele = povrsinaParcele,
                                AmbalazeZaduzeno = ambalazeZaduzeno,
                                Saldo = saldo
                            };
                            proizvodjaci.Add(proizvodjac);
                            if (Id == idOtkupljivaca)
                                Otkupljivac = proizvodjac;
                        }
                    }
                }
            }
            Proizvodjaci = proizvodjaci;
        }
        private void PopuniVoce()
        {
            List<Voce> voceLista = new List<Voce>();
            string connectionString = Pomocna.ConStr;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT ID, naziv, cena_i_klase, cena_ii_klase, cena_iii_klase FROM voce WHERE otkupno_mesto_ID = {Id}";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader.GetValue(0);
                            string naziv = (string)reader.GetValue(1);
                            decimal cenaIKlase = (decimal)reader.GetValue(2);
                            decimal cenaIIKlase = (decimal)reader.GetValue(3);
                            decimal cenaIIIKlase = (decimal)reader.GetValue(4);
                            Voce voce = new Voce(naziv, this, cenaIKlase, cenaIIKlase, cenaIIIKlase)
                            {
                                Id = id
                            };
                            voceLista.Add(voce);
                        }
                    }
                }
            }
            VoceLista = voceLista;
        }
        public static void Populate()
        {
            List<OtkupnoMesto> otkupnaMesta = new List<OtkupnoMesto>();
            string connectionString = Pomocna.ConStr;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT ID, ime, otkupljivac_ID FROM otkupno_mesto";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader.GetValue(0);
                            string ime = (string)reader.GetValue(1);
                            int? otkupljivacId = null;
                            if (!reader.IsDBNull(2))
                                otkupljivacId = (int?)reader.GetValue(2);
                            OtkupnoMesto otkupnoMesto = new OtkupnoMesto(ime)
                            {
                                Id = id
                            };
                            otkupnoMesto.PopuniProizvodjace(otkupljivacId);
                            otkupnoMesto.PopuniVoce();
                            otkupnaMesta.Add(otkupnoMesto);
                        }
                    }
                }
            }
            OtkupnaMesta = otkupnaMesta;
            Otkup.Populate();
        }
        public void Insert()
        {
            if (Id > 0)
                throw new Exception("Vec postoji u bazi podataka.");
            string connectionString = Pomocna.ConStr;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter ime = new SqlParameter("@ime", SqlDbType.NVarChar);
                    command.Parameters.Add(ime);
                    ime.Value = Ime;
                    if (!(Otkupljivac is null))
                    {
                        SqlParameter otkupljivacId = new SqlParameter("@otkupljivac_ID", SqlDbType.Int);
                        command.Parameters.Add(otkupljivacId);
                        otkupljivacId.Value = Otkupljivac.Id;
                        command.CommandText = "INSERT INTO otkupno_mesto (ime, otkupljivac_ID) VALUES (@ime, @otkupljivac_ID)";
                    }
                    else
                    {
                        command.CommandText = "INSERT INTO otkupno_mesto (ime) VALUES (@ime)";
                    }
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT IDENT_CURRENT('otkupno_mesto')";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        Id = int.Parse(reader.GetValue(0).ToString());
                    }
                }
            }
        }
        public void Update()
        {
            if (!(Id > 0))
                throw new Exception("Ne postoji u bazi podataka.");
            string connectionString = Pomocna.ConStr;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                    SqlParameter ime = new SqlParameter("@ime", SqlDbType.NVarChar);
                    SqlParameter otkupljivacId = new SqlParameter("@otkupljivac_ID", SqlDbType.Int);
                    command.Parameters.Add(id);
                    command.Parameters.Add(ime);
                    command.Parameters.Add(otkupljivacId);
                    id.Value = Id;
                    ime.Value = Ime;
                    otkupljivacId.Value = Otkupljivac.Id;
                    command.CommandText = "UPDATE otkupno_mesto SET ime = @ime, otkupljivac_ID = @otkupljivac_ID WHERE ID = @id";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
