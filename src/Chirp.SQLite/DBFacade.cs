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
        private readonly string _CHIRPDBPATH;

        private List<CheepViewModel> cheeps = new();

        public string Author { get; private set; }
        public string Message { get; private set; }
        public string Timestamp { get; private set; }

        public DBFacade(string CHIRPDBPATH)
        {
            _CHIRPDBPATH = CHIRPDBPATH;
        }

        public List<CheepViewModel> GetCheeps()
        {
            var query = @"SELECT * FROM message ORDER by message.pub_date desc";
            cheeps = GetCheepsFromQuery(query);
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
            using (var connection = new SqliteConnection($"Data Source={_CHIRPDBPATH}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                query = @"SELECT * FROM message ORDER by message.pub_date desc";
                command.CommandText = query;
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cheeps.Add(new CheepViewModel(Author, Message, Timestamp));
                }
            }
            return cheeps;
        }
    }
}