namespace Application.Services.Interfaces
{
    using Application.DTOs;

    using Domain.Models;

    using Microsoft.AspNetCore.Http;

    public interface ITrafficService
	{
        public Task<ProcessingResult> SaveDataAsync(IFormFile file);

        public Task<Traffic> UpdateTrafficAsync(TrafficUpdateDto dto);

        public Task DeleteTrafficAsync(Guid trafficId);
    }
}