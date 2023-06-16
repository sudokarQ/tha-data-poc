using CsvHelper.Configuration.Attributes;

namespace Domain.Models
{
	public class PedestrianCSVDate
	{
        [Index(1)]
        public string Day { get; set; }
        
        [Index(2)]
        public string Year { get; set; }

        [Index(3)]
        public int? SeventhAndParkCampus { get; set; }

        [Index(7)]
        public int? BlineConventionCentre { get; set; }

        [Index(10)]
        public int? JordanAndSeventh { get; set; }

        [Index(11)]
        public int? NCollegeAndRailRoad { get; set; }

        [Index(12)]
        public int? SWalnutAndWylie { get; set; }
    }
}

