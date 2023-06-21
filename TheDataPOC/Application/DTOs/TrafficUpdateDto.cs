using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class TrafficUpdateDto
    {
        public Guid TrafficId { get; set; }

        public string? County { get; set; }

        public string? Community { get; set; }

        public string? Street { get; set; }

        public string? At { get; set; }

        public string? Directory { get; set; }

        public string? Directions { get; set; }

        [Range(-90, 90)]
        public double? Latitude { get; set; }

        [Range(-180, 180)]
        public double? Longitude { get; set; }
    }
}
