using System;
using System.Data;
using System.Data.SqlClient;

namespace Hladnjaca
{
    class Proizvodjac
    {
        public int Id { get; set; }
        public decimal Saldo { get; set; }
        public double AmbalazeZaduzeno { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string JMBG { get; set; }
        public string Telefon { get; set; }
        public string Racun { get; set; }
        public double PovrsinaParcele { get; set; }
        public OtkupnoMesto OtkupnoMesto { get; set; }
        public void UnosZaduzenjaAmbalaze(double zaduzeno)
        {
            AmbalazeZaduzeno += zaduzeno;
            Pomocna.Loguj($"Unos zaduzenja ambalaze: {zaduzeno}({Ime} {Prezime}).");
        }
        public Proizvodjac(string ime, string prezime, OtkupnoMesto otkupnoMesto)
        {
            Saldo = 0;
            Ime = ime;
            Prezime = prezime;
            OtkupnoMesto = otkupnoMesto;
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
                    SqlParameter saldo = new SqlParameter("@saldo", SqlDbType.Decimal);
                    SqlParameter ambalazeZaduzeno = new SqlParameter("@ambalaze_zaduzeno", SqlDbType.Float);
                    SqlParameter ime = new SqlParameter("@ime", SqlDbType.NVarChar);
                    SqlParameter prezime = new SqlParameter("@prezime", SqlDbType.NVarChar);
                    SqlParameter jmbg = new SqlParameter("@jmbg", SqlDbType.VarChar);
                    SqlParameter telefon = new SqlParameter("@telefon", SqlDbType.VarChar);
                    SqlParameter racun = new SqlParameter("@racun", SqlDbType.VarChar);
                    SqlParameter povrsinaParcele = new SqlParameter("@povrsina_parcele", SqlDbType.Float);
                    SqlParameter otkupnoMestoId = new SqlParameter("@otkupno_mesto_ID", SqlDbType.Int);
                    command.Parameters.Add(saldo);
                    command.Parameters.Add(ambalazeZaduzeno);
                    command.Parameters.Add(ime);
                    command.Parameters.Add(prezime);
                    command.Parameters.Add(jmbg);
                    command.Parameters.Add(telefon);
                    command.Parameters.Add(racun);
                    command.Parameters.Add(povrsinaParcele);
                    command.Parameters.Add(otkupnoMestoId);
                    saldo.Value = Saldo;
                    ambalazeZaduzeno.Value = AmbalazeZaduzeno;
                    ime.Value = Ime;
                    prezime.Value = Prezime;
                    jmbg.Value = JMBG;
                    telefon.Value = Telefon;
                    racun.Value = Racun;
                    povrsinaParcele.Value = PovrsinaParcele;
                    otkupnoMestoId.Value = OtkupnoMesto.Id;
                    command.CommandText = "INSERT INTO proizvodjac (saldo, ambalaze_zaduzeno, ime, prezime, jmbg, telefon, racun, povrsina_parcele, otkupno_mesto_ID) VALUES (@saldo, @ambalaze_zaduzeno, @ime, @prezime, @jmbg, @telefon, @racun, @povrsina_parcele, @otkupno_mesto_ID)";
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT IDENT_CURRENT('proizvodjac')";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        Id = int.Parse(reader.GetValue(0).ToString());
                    }
                }
            }
        }
        public void AzurirajSaldo()
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
                    SqlParameter saldo = new SqlParameter("@saldo", SqlDbType.Decimal);
                    command.Parameters.Add(id);
                    command.Parameters.Add(saldo);
                    id.Value = Id;
                    saldo.Value = Saldo;
                    command.CommandText = "UPDATE proizvodjac SET saldo = @saldo WHERE ID = @id";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
