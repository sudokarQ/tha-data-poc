namespace Application.Services.Interfaces
{
    using Microsoft.AspNetCore.Http;

    public interface IUploadService
	{
        public Task<(int, int)> UploadFileAsync(IFormFile file);
    }
}

