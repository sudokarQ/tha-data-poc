namespace Domain.Models
{
    using Enums;
 
	public class Crash
	{
        public Guid Id { get; set; }

        public DateTime? Date { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public bool IsWeekend { get; set; }

        public int? Hour { get; set; }

        public InjuryType InjuryType { get; set; }

        public string? PrimaryFactor { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }
}

