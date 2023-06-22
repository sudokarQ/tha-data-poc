namespace Application.DTOs
{
    using System.ComponentModel.DataAnnotations;
    
    public class AuthDto
    {
        [Required]
        [RegularExpression(@"^[^\s@]+@([^\s@.,]+\.)+[^\s@.,]{2,}$", ErrorMessage = "Email is invalid")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
