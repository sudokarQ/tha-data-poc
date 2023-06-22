namespace Domain.Models
{
    using Sieve.Attributes;

    public class Traffic
	{
        public Guid TrafficId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string County { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Community { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Street { get; set; }

        public string StartPoint { get; set; }

        public string DestinationPoint { get; set; }

        public string Approach { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string At { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Directory { get; set; }

        public string Directions { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public double Latitude { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public double Longitude { get; set; }

        public int Latest { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime LatestDate { get; set; }
    }
}

