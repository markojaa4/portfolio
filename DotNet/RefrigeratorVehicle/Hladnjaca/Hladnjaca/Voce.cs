using System;
using System.Data;
using System.Data.SqlClient;

namespace Hladnjaca
{
    class Voce
    {
        public int Id { get; set; }
        public string Naziv { get; private set; }
        public OtkupnoMesto OtkupnoMesto { get; set; }
        public decimal CenaIKlase { get; set; }
        public decimal CenaIIKlase { get; set; }
        public decimal CenaIIIKlase { get; set; }
        public Voce(string naziv, OtkupnoMesto otkupnoMesto, decimal cenaIKlase, decimal cenaIIKlase, decimal cenaIIIKlase)
        {
            Naziv = naziv;
            OtkupnoMesto = otkupnoMesto;
            CenaIKlase = cenaIKlase;
            CenaIIKlase = cenaIIKlase;
            CenaIIIKlase = cenaIIIKlase;
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
                    SqlParameter naziv = new SqlParameter("@naziv", SqlDbType.NVarChar);
                    SqlParameter cenaIKlase = new SqlParameter("@cena_i_klase", SqlDbType.Decimal);
                    SqlParameter cenaIIKlase = new SqlParameter("@cena_ii_klase", SqlDbType.Decimal);
                    SqlParameter cenaIIIKlase = new SqlParameter("@cena_iii_klase", SqlDbType.Decimal);
                    SqlParameter otkupnoMestoId = new SqlParameter("@otkupno_mesto_ID", SqlDbType.Int);
                    command.Parameters.Add(naziv);
                    command.Parameters.Add(cenaIKlase);
                    command.Parameters.Add(cenaIIKlase);
                    command.Parameters.Add(cenaIIIKlase);
                    command.Parameters.Add(otkupnoMestoId);
                    naziv.Value = Naziv;
                    cenaIKlase.Value = CenaIKlase;
                    cenaIIKlase.Value = CenaIIKlase;
                    cenaIIIKlase.Value = CenaIIIKlase;
                    otkupnoMestoId.Value = OtkupnoMesto.Id;
                    command.CommandText = "INSERT INTO voce (naziv, cena_i_klase, cena_ii_klase, cena_iii_klase, otkupno_mesto_ID) VALUES (@naziv, @cena_i_klase, @cena_ii_klase, @cena_iii_klase, @otkupno_mesto_ID)";
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT IDENT_CURRENT('voce')";
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
                    SqlParameter naziv = new SqlParameter("@naziv", SqlDbType.NVarChar);
                    SqlParameter cenaIKlase = new SqlParameter("@cena_i_klase", SqlDbType.Decimal);
                    SqlParameter cenaIIKlase = new SqlParameter("@cena_ii_klase", SqlDbType.Decimal);
                    SqlParameter cenaIIIKlase = new SqlParameter("@cena_iii_klase", SqlDbType.Decimal);
                    SqlParameter otkupnoMestoId = new SqlParameter("@otkupno_mesto_ID", SqlDbType.Int);
                    command.Parameters.Add(id);
                    command.Parameters.Add(naziv);
                    command.Parameters.Add(cenaIKlase);
                    command.Parameters.Add(cenaIIKlase);
                    command.Parameters.Add(cenaIIIKlase);
                    command.Parameters.Add(otkupnoMestoId);
                    id.Value = Id;
                    naziv.Value = Naziv;
                    cenaIKlase.Value = CenaIKlase;
                    cenaIIKlase.Value = CenaIIKlase;
                    cenaIIIKlase.Value = CenaIIIKlase;
                    otkupnoMestoId.Value = OtkupnoMesto.Id;
                    command.CommandText = "UPDATE voce SET naziv = @naziv, cena_i_klase = @cena_i_klase, cena_ii_klase = @cena_ii_klase, cena_iii_klase = @tourns cena_iii_klase, otkupno_mesto_ID = @otkupno_mesto_ID WHERE ID = @id";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
