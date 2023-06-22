namespace Domain.Models
{
    using Enums;

    using Sieve.Attributes;

    public class Crash
	{
        public Guid Id { get; set; }

        [Sieve(CanSort = true)]
        public DateTime? Date { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int Year { get; set; }

        [Sieve(CanSort = true)]
        public int Month { get; set; }

        [Sieve(CanSort = true)]
        public int Day { get; set; }

        public bool IsWeekend { get; set; }

        [Sieve(CanSort = true)]
        public int? Hour { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public InjuryType InjuryType { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string PrimaryFactor { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Latitude { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Longitude { get; set; }
    }
}

