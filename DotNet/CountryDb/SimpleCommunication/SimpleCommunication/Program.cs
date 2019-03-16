using System;
using System.Data;
using System.Data.SqlClient;
using static System.Console;

namespace SimpleCommunication
{
    static class Globals
    {
        public const string connectionString = @"Server=(localdb)\mssqllocaldb;Database=countries;Trusted_Connection=True;MultipleActiveResultSets=True";
    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                WriteLine(Count() + " countries.");
                while (true)
                {
                    Write("Capital of: ");
                    WriteLine(SelectCountry(ReadLine()));
                }
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
            }
            WriteLine("Press any key to exit...");
            ReadKey();
        }
        static string SelectCountry(string country)
        {
            
            SqlCommand command;
            SqlDataReader dataReader;
            string query = "SELECT capital FROM country WHERE country_name = @country";
            string output = "";
            using (SqlConnection connection = new SqlConnection(Globals.connectionString))
            {
                connection.Open();
                using (command = new SqlCommand(query, connection))
                {
                    SqlParameter drzavaParam = new SqlParameter("@country", SqlDbType.NVarChar);
                    drzavaParam.Value = country;
                    command.Parameters.Add(drzavaParam);
                    using (dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            string res = (string)dataReader.GetValue(0);
                            if (res != "")
                                output += res + '\n';
                            else
                                output += "Not specified.\n";
                        }
                    }
                }
            }
            return output;
        }
        static void InsertContinent(string id, string name)
        {
            if (id.Length > 2)
                throw new ArgumentException("ID should contain two capital letters.");
            string query = "INSERT INTO continent (ID, continent_name) VALUES (@id, @name)";
            using (SqlConnection connection = new SqlConnection(Globals.connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter idParam = new SqlParameter("@id", SqlDbType.VarChar);
                    SqlParameter nazivParam = new SqlParameter("@name", SqlDbType.NVarChar);
                    idParam.Value = id;
                    nazivParam.Value = name;
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(nazivParam);
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
            }
        }
        static int Count()
        {
            string query = "SELECT COUNT(*) FROM country";
            int result = 0;
            using (SqlConnection connection = new SqlConnection(Globals.connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    result = (int)command.ExecuteScalar();
                }
            }
            return result;
        }
    }
}
