using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static System.Console;

namespace Players
{
    class Player
    {
        public int Ranking { get; private set; }
        public bool Tied { get; private set; }
        public string Country { get; private set; }
        public string FullName { get; private set; }
        public int Points { get; private set; }
        public int TournsPlayed { get; private set; }
        public Player(int ranking, bool tied, string country, string fullName, int points, int tournsPlayed)
        {
            Ranking = ranking;
            Tied = tied;
            Country = country;
            FullName = fullName;
            Points = points;
            TournsPlayed = tournsPlayed;
        }
    }
    static class Parser
    {
        static string GetTbody(string htmlContent)
        {
            int start = htmlContent.IndexOf("<tbody>");
            int end = htmlContent.IndexOf("</tbody>");
            htmlContent = htmlContent.Substring(start, end - start);
            return htmlContent;
        }
        static string GetRow(string htmlContent)
        {
            int start = htmlContent.IndexOf("<tr>");
            int end = htmlContent.IndexOf("</tr>");
            htmlContent = htmlContent.Substring(start, end - start);
            return htmlContent;
        }
        static string GetRanking(string htmlContent)
        {
            string startStr = "class=\"rank-cell\">";
            int start = htmlContent.IndexOf(startStr);
            int end = htmlContent.IndexOf("</td>");
            htmlContent = htmlContent.Substring(start, end - start);
            htmlContent = htmlContent.Replace(startStr, "");
            htmlContent = htmlContent.Trim();
            return htmlContent;
        }
        static string GetCountry(string htmlContent)
        {
            string startStr = "alt=\"";
            int start = htmlContent.IndexOf(startStr);
            htmlContent = htmlContent.Substring(start);
            htmlContent = htmlContent.Replace(startStr, "");
            int end = htmlContent.IndexOf("\"");
            htmlContent = htmlContent.Substring(0, end);
            htmlContent = htmlContent.Trim();
            return htmlContent;
        }
        static string GetPlayer(string htmlContent)
        {
            int start = htmlContent.IndexOf("player-cell");
            htmlContent = htmlContent.Substring(start);
            string startStr = "data-ga-label=\"";
            start = htmlContent.IndexOf(startStr);
            htmlContent = htmlContent.Substring(start);
            htmlContent = htmlContent.Replace(startStr, "");
            int end = htmlContent.IndexOf("\"");
            htmlContent = htmlContent.Substring(0, end);
            htmlContent = htmlContent.Trim();
            return htmlContent;
        }
        static string GetPoints(string htmlContent)
        {
            int start = htmlContent.IndexOf("points-cell");
            htmlContent = htmlContent.Substring(start);
            start = htmlContent.IndexOf("<a");
            htmlContent = htmlContent.Substring(start);
            int end = htmlContent.IndexOf(">");
            string openTag = htmlContent.Substring(start, end - start + 1);
            end = htmlContent.IndexOf("</a>");
            htmlContent = htmlContent.Substring(start, end - start);
            htmlContent = htmlContent.Replace(openTag, "");
            htmlContent = htmlContent.Trim();
            htmlContent = htmlContent.Replace(",", "");
            return htmlContent;
        }
        static string GetTourns(string htmlContent)
        {
            int start = htmlContent.IndexOf("tourn-cell");
            htmlContent = htmlContent.Substring(start);
            start = htmlContent.IndexOf("<a");
            htmlContent = htmlContent.Substring(start);
            int end = htmlContent.IndexOf(">");
            string openTag = htmlContent.Substring(start, end - start + 1);
            end = htmlContent.IndexOf("</a>");
            htmlContent = htmlContent.Substring(start, end - start);
            htmlContent = htmlContent.Replace(openTag, "");
            htmlContent = htmlContent.Trim();
            return htmlContent;
        }
        public static Queue<Player> Parse(string htmlContent)
        {
            Queue<Player> players = new Queue<Player>();
            htmlContent = GetTbody(htmlContent);
            while (htmlContent.Contains("<tr>"))
            {
                string row = GetRow(htmlContent);
                htmlContent = htmlContent.Replace(row + "</tr>", "");
                string rankingStr = GetRanking(row);
                bool tied = false;
                if (rankingStr.Contains("T"))
                {
                    rankingStr = rankingStr.Replace("T", "");
                    tied = true;
                }
                int ranking = int.Parse(rankingStr);
                string country = GetCountry(row);
                string fullName = GetPlayer(row);
                int points = int.Parse(GetPoints(row));
                int tounsPlayed = int.Parse(GetTourns(row));
                Player player = new Player(ranking, tied, fullName, country, points, tounsPlayed);
                players.Enqueue(player);
            }
            return players;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            File.WriteAllText("page-source.html", GetContent("https://www.atptour.com/en/rankings/singles?rankDate=2019-03-04&rankRange=1-5000"));
            string page = File.ReadAllText("page-source.html");
            //string page = GetContent("https://www.atptour.com/en/rankings/singles?rankDate=2019-03-04&rankRange=1-5000");
            //Queue<Player> players = Parser.Parse(page);
            //WriteLine(PopulateDb(players));
            ReadKey();
        }
        static string GetContent(string address)
        {
            WebRequest request = WebRequest.Create(address);
            string responseFromServer = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception($"Error: {response.StatusCode}");
                using (Stream dataStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        responseFromServer = reader.ReadToEnd();
                    }
                }
            }
            return responseFromServer;
        }
        static int PopulateDb(Queue<Player> players)
        {
            string connectionString = @"Server=(localdb)\mssqllocaldb;Database=drzave;Trusted_Connection=True;MultipleActiveResultSets=True";
            string query = "INSERT INTO player (ranking, tied, full_name, country, points, tournaments_played) VALUES (@ranking, @tied, @country, @fname, @points, @tourns)";
            int inserted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    SqlParameter rankingParam = new SqlParameter("@ranking", SqlDbType.NVarChar);
                    SqlParameter tiedParam = new SqlParameter("@tied", SqlDbType.NVarChar);
                    SqlParameter countryParam = new SqlParameter("@country", SqlDbType.NVarChar);
                    SqlParameter fullNameParam = new SqlParameter("@fname", SqlDbType.NVarChar);
                    SqlParameter tournsParam = new SqlParameter("@tourns", SqlDbType.NVarChar);
                    SqlParameter pointsParam = new SqlParameter("@points", SqlDbType.NVarChar);
                    command.Parameters.Add(rankingParam);
                    command.Parameters.Add(tiedParam);
                    command.Parameters.Add(countryParam);
                    command.Parameters.Add(fullNameParam);
                    command.Parameters.Add(tournsParam);
                    command.Parameters.Add(pointsParam);
                    command.CommandText = query;
                    while (players.Count > 0)
                    {
                        Player player = players.Dequeue();
                        rankingParam.Value = player.Ranking;
                        tiedParam.Value = player.Tied;
                        countryParam.Value = player.Country;
                        fullNameParam.Value = player.FullName;
                        tournsParam.Value = player.TournsPlayed;
                        pointsParam.Value = player.Points;
                        command.ExecuteNonQuery();
                        inserted++;
                    }
                }
            }
            return inserted;
        }
    }
}
