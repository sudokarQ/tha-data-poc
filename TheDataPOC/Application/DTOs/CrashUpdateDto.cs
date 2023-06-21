namespace Application.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class CrashUpdateDto
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }

        [Range(0, 23, ErrorMessage = "Hour must be between 0 and 23.")]
        public int? Hour { get; set; }

        public string? PrimaryFactor { get; set; }

        [Range(-90, 90)]
        public double? Latitude { get; set; }

        [Range(-180, 180)]
        public double? Longitude { get; set; }
    }
}
