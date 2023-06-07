namespace Application.Services.Interfaces
{
    using Microsoft.AspNetCore.Http;

    public interface ICrashService
	{
        public Task<(int, int)> DataProcessing(IFormFile file);
    }
}

