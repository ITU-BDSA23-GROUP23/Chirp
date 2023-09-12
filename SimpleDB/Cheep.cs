using CsvHelper.Configuration.Attributes;
namespace SimpleDB;


public class Cheep
        {
        [Index(0)]
        public string Id { get; set; }

        [Index(1)]
        public string Name { get; set; }

        [Index(2)]
        public long Time{get; set;}
        }