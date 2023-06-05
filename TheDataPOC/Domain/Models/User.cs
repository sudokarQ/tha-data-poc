namespace Domain.Models
{
	public class User
	{
		public string UserName { get; set; }

		public string NormalizedUserName { get; set; }

		public string Email { get; set; }

		public string PasswordHash { get; set; }
	}
}

