namespace Domain.Models
{
    using CsvHelper.Configuration.Attributes;

    [Delimiter(",")]
    public class TrafficCSV
	{
        public Guid TrafficId { get; set; } = Guid.NewGuid();

        [Name("County")]
        public string County { get; set; }

        [Name("Community")]
        public string Community { get; set; }

        [Name("On")]
        [Optional]
        public string Street { get; set; }

        [Name("From")]
        [Optional]
        public string StartPoint { get; set; }

        [Name("To")]
        [Optional]
        public string DestinationPoint { get; set; }

        [Name("Approach")]
        [Optional]
        public string Approach { get; set; }

        [Name("At")]
        public string At { get; set; }

        [Name("Dir")]
        public string Directory { get; set; }

        [Name("Directions")]
        [Optional]
        public string Directions { get; set; }

        [Name("Latitude")]
        [Default(0)]
        public double Latitude { get; set; }

        [Name("Longitude")]
        [Default(0)]
        public double Longitude { get; set; }

        [Name("Latest")]
        [Default(0)]
        public int Latest { get; set; }

        [Name("Latest Date")]
        [Default("1/1/1000")]
        public string LatestDate { get; set; }
    }
}

