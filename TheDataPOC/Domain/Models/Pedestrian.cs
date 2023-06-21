using Sieve.Attributes;

namespace Domain.Models
{
	public class Pedestrian
	{
        public Guid Id { get; set; }

        [Sieve(CanSort = true)]
        public DateTime? Date { get; set; }

        [Sieve(CanSort = true)]
        public int? SeventhAndParkCampus { get; set; }

        [Sieve(CanSort = true)]
        public int? BlineConventionCentre { get; set; }

        [Sieve(CanSort = true)]
        public int? JordanAndSeventh { get; set; }

        [Sieve(CanSort = true)]
        public int? NCollegeAndRailRoad { get; set; }

        [Sieve(CanSort = true)]
        public int? SWalnutAndWylie { get; set; }
    }
}

