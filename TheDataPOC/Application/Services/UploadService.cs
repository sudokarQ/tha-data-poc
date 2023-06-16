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

        private readonly IPedestrianService pedestrianService;

        public UploadService(ICrashService crashService, ITrafficService trafficService, IPedestrianService pedestrianService)
		{
            this.crashService = crashService;
            this.trafficService = trafficService;
            this.pedestrianService = pedestrianService;
		}

        public async Task<ProcessingResult> UploadFileAsync(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) != ".csv")
            {
                throw new Exception("You can upload only .csv files");                                              
            }

            if (file.FileName.StartsWith("crash"))
            {
                return await crashService.DataProcessing(file);
            }

            if (file.FileName.Contains("traffic-count"))
            {
                return await trafficService.SaveDataAsync(file);
            }

            if (file.FileName.Contains("pedestrian"))
            {
                return await pedestrianService.DataProcessing(file);
            }

            return await Task.FromResult(new ProcessingResult { AllRows = 0, UploadedRows = 0,});;
        }
    }
}

