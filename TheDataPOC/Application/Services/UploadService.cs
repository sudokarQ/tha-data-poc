namespace Application.Services
{
    using System.Threading.Tasks;

    using Interfaces;

    using Microsoft.AspNetCore.Http;

    public class UploadService : IUploadService
	{
        private readonly ICrashService crashService;
		public UploadService(ICrashService crashService)
		{
            this.crashService = crashService;
		}

        public Task<(int, int)> UploadFileAsync(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) != ".csv")
            {
                throw new Exception("You can upload only .csv files");
            }

            if (file.FileName.StartsWith("crash"))
            {
                return crashService.DataProcessing(file);
            }

            return Task.FromResult((0,0));
        }
    }
}

