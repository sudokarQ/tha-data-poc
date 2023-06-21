namespace Domain.Models
{
    using Enums;

    using Sieve.Attributes;

    public class Crash
	{
        public Guid Id { get; set; }

        public DateTime? Date { get; set; }

        [Sieve(CanFilter = true)]
        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public bool IsWeekend { get; set; }

        public int? Hour { get; set; }

        [Sieve(CanFilter = true)]
        public InjuryType InjuryType { get; set; }

        [Sieve(CanFilter = true)]
        public string PrimaryFactor { get; set; }

        [Sieve(CanFilter = true)]
        public string Latitude { get; set; }

        [Sieve(CanFilter = true)]
        public string Longitude { get; set; }
    }
}

