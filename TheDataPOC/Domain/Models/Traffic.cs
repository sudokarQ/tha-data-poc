namespace Domain.Models
{
	public class Traffic
	{
        public Guid TrafficId { get; set; }

        public string County { get; set; }

        public string Community { get; set; }

        public string Street { get; set; }

        public string StartPoint { get; set; }

        public string DestinationPoint { get; set; }

        public string Approach { get; set; }

        public string At { get; set; }

        public string Directory { get; set; }

        public string Directions { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Latest { get; set; }

        public DateTime LatestDate { get; set; }
    }
}

