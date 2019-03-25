using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static System.Console;

namespace PlayersClient
{
    static class Global
    {
        public static readonly string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=countries;Trusted_Connection=True;MultipleActiveResultSets=True";
    }
    interface IDbEntity
    {
        void Refresh();
        void Delete();
        void Insert();
        void Update();
        void Upsert();
    }
    class Player : IDbEntity
    {
        private int DatabaseId { get; set; }
        public int Ranking { get; private set; }
        public string Country { get; set; }
        public string FullName { get; set; }
        public int Points { get; set; }
        public int TournsPlayed { get; set; }
        public static List<Player> ListFromCountry(int countryId)
        {
            List<Player> players = new List<Player>();
            string connectionString = Global.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int)
                    {
                        Value = countryId
                    };
                    command.Parameters.Add(idParam);
                    command.CommandText = "SELECT player.ID, country.country_name, player.full_name, player.points, player.tournaments_played FROM player INNER JOIN country ON player.country_ID = country.ID WHERE country.ID = @id";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int databaseId = (int)reader.GetValue(0);
                            string country = (string)reader.GetValue(1);
                            string fullName = (string)reader.GetValue(2);
                            int points = (int)reader.GetValue(3);
                            int tournsPlayed = (int)reader.GetValue(4);
                            Player player = new Player(country, fullName, points, tournsPlayed)
                            {
                                DatabaseId = databaseId
                            };
                            players.Add(player);
                        }
                    }
                }
            }
            return players;
        }
        public static void DetermineRanking(List<Player> allPlayers)
        {
            allPlayers.Sort((a, b) => a.Points.CompareTo(b.Points));
            for (int i = 0; i < allPlayers.Count; i++)
                allPlayers[i].Ranking = i + 1;
        }
        public void Upsert()
        {
            if (DatabaseId > 0)
                Update();
            else
                Insert();
        }
        public void Update()
        {
            if (!(DatabaseId > 0))
                throw new Exception("Player doesn't exist in database.");
            string connectionString = Global.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int);
                    SqlParameter countryParam = new SqlParameter("@country", SqlDbType.NVarChar);
                    SqlParameter fullNameParam = new SqlParameter("@fname", SqlDbType.NVarChar);
                    SqlParameter tournsParam = new SqlParameter("@tourns", SqlDbType.Int);
                    SqlParameter pointsParam = new SqlParameter("@points", SqlDbType.Int);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(countryParam);
                    command.Parameters.Add(fullNameParam);
                    command.Parameters.Add(tournsParam);
                    command.Parameters.Add(pointsParam);
                    idParam.Value = DatabaseId;
                    countryParam.Value = Country;
                    fullNameParam.Value = FullName;
                    tournsParam.Value = TournsPlayed;
                    pointsParam.Value = Points;
                    command.CommandText = "UPDATE player_country_view SET full_name = @fname, country_name = @country, points = @points, tournaments_played = @tourns WHERE player_ID = @id";
                    command.ExecuteNonQuery();
                }
            }
        }
        public void Insert()
        {
            if (DatabaseId > 0)
                throw new Exception("Player already exists in database.");
            string connectionString = Global.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter countryParam = new SqlParameter("@country", SqlDbType.NVarChar);
                    SqlParameter fullNameParam = new SqlParameter("@fname", SqlDbType.NVarChar);
                    SqlParameter tournsParam = new SqlParameter("@tourns", SqlDbType.Int);
                    SqlParameter pointsParam = new SqlParameter("@points", SqlDbType.Int);
                    command.Parameters.Add(countryParam);
                    command.Parameters.Add(fullNameParam);
                    command.Parameters.Add(tournsParam);
                    command.Parameters.Add(pointsParam);
                    countryParam.Value = Country;
                    fullNameParam.Value = FullName;
                    tournsParam.Value = TournsPlayed;
                    pointsParam.Value = Points;
                    command.CommandText = "INSERT INTO player_country_view (country_name, full_name, points, tournaments_played) VALUES (@country, @fname, @points, @tourns)";
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT IDENT_CURRENT('player')";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        DatabaseId = int.Parse(reader.GetValue(0).ToString());
                    }
                }
            }
        }
        public void Delete()
        {
            if (!(DatabaseId > 0))
                throw new Exception("Player doesn't exist in database.");
            string connectionString = Global.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int)
                    {
                        Value = DatabaseId
                    };
                    command.Parameters.Add(idParam);
                    command.CommandText = "DELETE FROM player WHERE id = @id";
                    command.ExecuteNonQuery();
                    DatabaseId = 0;
                }
            }
        }
        public void Refresh()
        {
            if (!(DatabaseId > 0))
                throw new Exception("Player doesn't exist in database.");
            string connectionString = Global.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int)
                    {
                        Value = DatabaseId
                    };
                    command.Parameters.Add(idParam);
                    command.CommandText = "SELECT country_name, full_name, points, tournaments_played FROM player_country_view WHERE player_ID = @id";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new Exception("Player doesn't exist in database.");
                        Country = (string)reader.GetValue(0);
                        FullName = (string)reader.GetValue(1);
                        Points = (int)reader.GetValue(2);
                        TournsPlayed = (int)reader.GetValue(3);
                    }
                }
            }
        }
        public Player(string country, string fullName, int points, int tournsPlayed)
        {
            Country = country;
            FullName = fullName;
            Points = points;
            TournsPlayed = tournsPlayed;
        }
        public Player(int databaseId)
        {
            string connectionString = Global.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int)
                    {
                        Value = databaseId
                    };
                    command.Parameters.Add(idParam);
                    command.CommandText = "SELECT country_name, full_name, points, tournaments_played FROM player_country_view WHERE player_ID = @id";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new Exception("Player doesn't exist in database.");
                        DatabaseId = databaseId;
                        Country = (string)reader.GetValue(0);
                        FullName = (string)reader.GetValue(1);
                        Points = (int)reader.GetValue(2);
                        TournsPlayed = (int)reader.GetValue(3);
                    }
                }
            }
        }
    }
    class Country : IDbEntity
    {
        private int DatabaseId { get; set; }
        private string iso2;
        public string Iso2
        {
            get => iso2;
            set
            {
                if (value.Length != 2)
                    throw new Exception("Iso2 must contain 2 characters");
                iso2 = value;
            }
        }
        private string iso3;
        public string Iso3
        {
            get => iso3;
            set
            {
                if (value.Length != 3)
                    throw new Exception("Iso3 must contain 3 characters");
                iso3 = value;
            }
        }
        public string Name { get; set; }
        public long Population { get; set; }
        public string Capital { get; set; }
        private List<Player> _players;
        public IReadOnlyList<Player> Players
        {
            get => _players.AsReadOnly();
        }
        public static Dictionary<string, Country> GetAll()
        {
            Dictionary<string, Country> countries = new Dictionary<string, Country>();
            string connectionString = Global.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT country_name, country_population, capital, ID FROM country";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = (string)reader.GetValue(0);
                            long population = (long)reader.GetValue(1);
                            string capital = (string)reader.GetValue(2);
                            int id = (int)reader.GetValue(3);
                            Country country = new Country(name, population, capital, Player.ListFromCountry(id))
                            {
                                DatabaseId = id
                            };
                            countries.Add(name.ToLower(), country);
                        }
                    }
                }
            }
            return countries;
        }
        public void Upsert()
        {
            if (DatabaseId > 0)
                Update();
            else
                Insert();
        }
        public void Update()
        {
            if (!(DatabaseId > 0))
                throw new Exception("Country doesn't exist in database.");
            string connectionString = Global.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int);
                    SqlParameter nameParam = new SqlParameter("@name", SqlDbType.NVarChar);
                    SqlParameter popParam = new SqlParameter("@pop", SqlDbType.BigInt);
                    SqlParameter capParam = new SqlParameter("@cap", SqlDbType.NVarChar);
                    SqlParameter iso2Param = new SqlParameter("@iso2", SqlDbType.Char);
                    SqlParameter iso3Param = new SqlParameter("@iso3", SqlDbType.Char);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(popParam);
                    command.Parameters.Add(capParam);
                    command.Parameters.Add(iso2Param);
                    command.Parameters.Add(iso3Param);
                    idParam.Value = DatabaseId;
                    nameParam.Value = Name;
                    popParam.Value = Population;
                    capParam.Value = Capital;
                    iso2Param.Value = Iso2;
                    iso3Param.Value = Iso3;
                    command.CommandText = "UPDATE country SET country_name = @name, country_population = @pop, capital = @cap, iso2 = @iso2, iso3 = @iso3 WHERE ID = @id";
                    command.ExecuteNonQuery();
                }
            }
        }
        public void Insert()
        {
            if (DatabaseId > 0)
                throw new Exception("Country already exists in database.");
            string connectionString = Global.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter nameParam = new SqlParameter("@name", SqlDbType.NVarChar);
                    SqlParameter popParam = new SqlParameter("@pop", SqlDbType.BigInt);
                    SqlParameter capParam = new SqlParameter("@cap", SqlDbType.NVarChar);
                    SqlParameter iso2Param = new SqlParameter("@iso2", SqlDbType.Char);
                    SqlParameter iso3Param = new SqlParameter("@iso3", SqlDbType.Char);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(popParam);
                    command.Parameters.Add(capParam);
                    command.Parameters.Add(iso2Param);
                    command.Parameters.Add(iso3Param);
                    nameParam.Value = Name;
                    popParam.Value = Population;
                    capParam.Value = Capital;
                    iso2Param.Value = Iso2;
                    iso3Param.Value = Iso3;
                    command.CommandText = "INSERT INTO country (country_name, country_population, capital, iso2, iso3) VALUES (@name, @pop, @cap, @iso2, @iso3)";
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT IDENT_CURRENT('country')";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        DatabaseId = int.Parse(reader.GetValue(0).ToString());
                    }
                }
            }
        }
        public void Delete()
        {
            if (!(DatabaseId > 0))
                throw new Exception("Country doesn't exist in database.");
            string connectionString = Global.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int)
                    {
                        Value = DatabaseId
                    };
                    command.Parameters.Add(idParam);
                    command.CommandText = "DELETE FROM country WHERE id = @id";
                    command.ExecuteNonQuery();
                    DatabaseId = 0;
                }
            }
        }
        public void Refresh()
        {
            if (!(DatabaseId > 0))
                throw new Exception("Country doesn't exist in database.");
            string connectionString = Global.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int)
                    {
                        Value = DatabaseId
                    };
                    command.Parameters.Add(idParam);
                    command.CommandText = "SELECT country_name, country_population, capital, iso2, iso3 FROM country WHERE ID = @id";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new Exception("Country doesn't exist in database.");
                        Name = (string)reader.GetValue(0);
                        Population = (long)reader.GetValue(1);
                        Capital = (string)reader.GetValue(2);
                        Iso2 = (string)reader.GetValue(3);
                        Iso3 = (string)reader.GetValue(4);
                        _players = Player.ListFromCountry(DatabaseId);
                    }
                }
            }
        }
        public Country(string name, long population, string capital, List<Player> players)
        {
            Name = name;
            Population = population;
            Capital = capital;
            _players = players;
        }
        public Country(int databaseId)
        {
            string connectionString = Global.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int)
                    {
                        Value = databaseId
                    };
                    command.Parameters.Add(idParam);
                    command.CommandText = "SELECT country_name, country_population, capital, iso2, iso3 FROM country WHERE ID = @id";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new Exception("Country doesn't exist in database.");
                        DatabaseId = databaseId;
                        Name = (string)reader.GetValue(0);
                        Population = (long)reader.GetValue(1);
                        Capital = (string)reader.GetValue(2);
                        Iso2 = (string)reader.GetValue(3);
                        Iso3 = (string)reader.GetValue(4);
                        _players = Player.ListFromCountry(databaseId);
                    }
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Country> dict = Country.GetAll();
            while (true)
            {
                WriteLine("Enter country name:");
                string query = ReadLine().ToLower();
                while (!dict.ContainsKey(query))
                {
                    WriteLine("Not found. Try again:");
                    query = ReadLine();
                }
                Country country = dict[query];
                WriteLine($"Population: {country.Population}, capital: {country.Capital}.");
                WriteLine($"Professional tennis players in {country.Name}: {country.Players.Count}.");
                foreach (Player player in country.Players)
                    WriteLine($"-{player.FullName.PadRight(28)} points: {player.Points.ToString().PadRight(10)} tournaments played: {player.TournsPlayed}");
                WriteLine();
            }
        }
    }
}
