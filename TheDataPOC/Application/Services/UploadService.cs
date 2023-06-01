namespace Application.Services
{
    using System.Threading.Tasks;

    using Interfaces;

    using Microsoft.AspNetCore.Http;

    public class UploadService : IUploadService
	{
		public UploadService()
		{
		}

        public Task<int> UploadFileAsync(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) == ".csv")
            {
                return Task.FromResult(0);
            }

            throw new Exception("You can upload only .csv files");
        }
    }
}

