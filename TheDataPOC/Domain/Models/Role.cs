namespace Domain.Models
{
	public class Role
	{
        public int Id { get; set; }

        public string RoleName { get; set; }

        public string NormalizedRoleName {get; set;}

        public List<UserRole> UserRoles {get; set;}
    }
}

