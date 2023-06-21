namespace Application.Services.Interfaces
{
    using Application.DTOs;

    using Domain.Models;

    using Microsoft.AspNetCore.Http;

    public interface IPedestrianService
    {
        public Task<ProcessingResult> DataProcessing(IFormFile file);

        public Task<Pedestrian> UpdatePedestrianAsync(PedestrianUpdateDto dto);

        public Task DeletePedestrianAsync(Guid pedestrianId);
    }
}

