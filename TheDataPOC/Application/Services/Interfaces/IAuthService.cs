namespace Application.Services.Interfaces
{
    using DTOs;

    public interface IAuthService
    {
        Task<bool> ValidateUser(AuthDto dto);

        Task<string> CreateToken();
    }
}
