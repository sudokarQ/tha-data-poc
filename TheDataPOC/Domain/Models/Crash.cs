namespace Domain.Models
{
	public class Crash
	{
        public Guid CrashId { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public string Weekend { get; set; }

        public int Hour { get; set; }

        public int InjuryType { get; set; }

        public string PrimaryFactor { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}

