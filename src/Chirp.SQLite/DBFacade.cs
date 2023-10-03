using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using Chirp.Razor;

namespace Chirp.Razor
{
    public class DBFacade
    {
        public record CheepViewModel(string Author, string Message, string Timestamp);
        private static readonly string CHIRPDBPATH = Environment.GetEnvironmentVariable("CHIRPDBPATH") is not null?
                                                     Environment.GetEnvironmentVariable("CHIRPDBPATH"):
                                                     "/tmp/chirp.db";

        private List<CheepViewModel> cheeps = new();

        public DBFacade()
        {
            if (!File.Exists(CHIRPDBPATH)) {
                File.Create(CHIRPDBPATH);
            }
        }

        public List<CheepViewModel> GetCheeps()
        {
            var query = @"SELECT * FROM message ORDER by message.pub_date desc";
            cheeps = GetCheepsFromQuery(query);
            Console.WriteLine("", cheeps.Count);
            return cheeps;
        }

        public List<CheepViewModel> GetCheepsFromAuthor(string author)
        {
            // filter by the provided author name
            string query = $"SELECT * FROM message WHERE author_id = (SELECT user_id FROM user WHERE username = '{author}');";
            cheeps = GetCheepsFromQuery(query);
            return cheeps;
        }
        private List<CheepViewModel> GetCheepsFromQuery(string query)
        {
            using (var connection = new SqliteConnection($"Data Source={CHIRPDBPATH}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                //query = @"SELECT * FROM message ORDER by message.pub_date desc";
                command.CommandText = query;
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var dataRecord = (IDataRecord)reader;
                    Console.WriteLine("{0}, {1}, {2}, {3}", dataRecord[0], dataRecord[1], dataRecord[2], dataRecord[3]);
                    //cheeps.Add(new CheepViewModel(dataRecord[1], Message, Timestamp));
                }
            }
            return cheeps;
        }
    }
}