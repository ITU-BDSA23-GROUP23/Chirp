
using CsvHelper.Configuration.Attributes;



public class Cheep
{
        [Index(0)]
        public string Author { get; set; }

        [Index(1)]
        public string Message { get; set; }

        [Index(2)]
        public long Timestamp { get; set; }
}