namespace API.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    
    using Domain.Models;

    public class LogInViewModel
    {
        [Required(ErrorMessage = "Write login")]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: User.UserNameMaxLength,
            MinimumLength = User.UserNameMinLength,
            ErrorMessage = "Wrong name")]
        [Display(Name = "Login")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "This field required password.")]
        [DataType(DataType.Password)]
        [StringLength(User.UserPasswordMaxLength,
            MinimumLength = User.UserPasswordMinLength,
            ErrorMessage = "Not enough symbols in your password")]
        public string Password { get; set; }
    }
}