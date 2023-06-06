namespace Application.Services.Interfaces
{
    using Domain.Models;

    using Microsoft.AspNetCore.Http;

    public interface ITrafficService
	{
        public Task<ProcessingResult> SaveDataAsync(IFormFile file);
    }
}