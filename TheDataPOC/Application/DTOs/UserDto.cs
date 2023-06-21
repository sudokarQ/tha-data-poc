namespace Application.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public IList<string> UserRoles { get; set; }
    }
}
