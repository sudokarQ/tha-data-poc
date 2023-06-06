namespace Application.Services.Interfaces
{
    using Domain.Models;

    using Microsoft.AspNetCore.Http;

    public interface IUploadService
	{
        public Task<ProcessingResult> UploadFileAsync(IFormFile file);
    }
}

