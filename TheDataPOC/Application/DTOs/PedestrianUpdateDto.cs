using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class PedestrianUpdateDto
    {
        public Guid Id { get; set; }

        [Range(0, int.MaxValue)]
        public int? SeventhAndParkCampus { get; set; }

        [Range(0, int.MaxValue)]
        public int? BlineConventionCentre { get; set; }

        [Range(0, int.MaxValue)]
        public int? JordanAndSeventh { get; set; }

        [Range(0, int.MaxValue)]
        public int? NCollegeAndRailRoad { get; set; }

        [Range(0, int.MaxValue)]
        public int? SWalnutAndWylie { get; set; }
    }
}
