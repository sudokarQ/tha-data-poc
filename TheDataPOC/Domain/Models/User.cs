namespace Domain.Models
{
	public class User
	{
		public const int UserNameMinLength = 2;

		public const int UserNameMaxLength = 16;

		public const int UserPasswordMinLength = 6;

		public const int UserPasswordMaxLength = 20;

		public const int UserEmailMinLength = 8;
		
		public const int UserEmailMaxLength = 25;
		
		
		public int Id { get; set; }
		
		public string UserName { get; set; }

		public string NormalizedUserName { get; set; }

		public string Email { get; set; }

		public string PasswordHash { get; set; }

		public List<UserRole> UserRoles {get; set;}
	}
}

