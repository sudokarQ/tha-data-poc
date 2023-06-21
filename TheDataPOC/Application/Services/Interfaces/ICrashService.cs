namespace Application.Services.Interfaces
{
    using Application.DTOs;

    using Domain.Models;

    using Microsoft.AspNetCore.Http;

    public interface ICrashService
	{
        public Task<ProcessingResult> DataProcessing(IFormFile file);

        public Task DeleteCrashAsync(Guid crashId);

        public Task<Crash> UpdateCrashAsync(CrashUpdateDto dto);
    }
}

