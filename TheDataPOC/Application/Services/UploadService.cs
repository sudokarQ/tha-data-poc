namespace Application.Services
{
    using System.Threading.Tasks;

    using Domain.Models;

    using Interfaces;

    using Microsoft.AspNetCore.Http;

    public class UploadService : IUploadService
	{
        private readonly ICrashService crashService;

        private readonly ITrafficService trafficService;

        public UploadService(ICrashService crashService, ITrafficService trafficService)
		{
            this.crashService = crashService;
            this.trafficService = trafficService;
		}

        public Task<ProcessingResult> UploadFileAsync(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) != ".csv")
            {
                throw new Exception("You can upload only .csv files");                                              
            }

            if (file.FileName.StartsWith("crash"))
            {
                return crashService.DataProcessing(file);
            }

            if (file.FileName.Contains("traffic-count"))
            {
                return trafficService.SaveDataAsync(file);
            }

            return Task.FromResult(new ProcessingResult { AllRows = 0, UploadedRows = 0,});;
        }
    }
}

