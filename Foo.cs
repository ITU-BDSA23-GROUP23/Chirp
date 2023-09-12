using CsvHelper.Configuration.Attributes;

public class Foo
        {
        [Index(0)]
        public string Id { get; set; }

        [Index(1)]
        public string Name { get; set; }

        [Index(2)]
        public string Time{get; set;}
        }