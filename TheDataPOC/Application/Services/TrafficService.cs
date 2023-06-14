namespace Application.Services
{
    using System.Data;
    using System.Globalization;

    using AutoMapper;
    using Application.CSVHelper;

    using CsvHelper;
    using CsvHelper.Configuration;

    using Domain.Models;

    using Interfaces;

    using Infrastructure.UnitOfWork;

    using Microsoft.AspNetCore.Http;
    
    public class TrafficService : ITrafficService
	{
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        private readonly DateTime DefaultDate = new DateTime(1000, 1, 1);

        public TrafficService(IUnitOfWork unitOfWork, IMapper mapper)
		{
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
		}

        public async Task<ProcessingResult> SaveDataAsync(IFormFile file)
        {
            var trafficCSVs = GetTrafficCSVs(file);

            var rowCount = trafficCSVs.Count();
                        
            trafficCSVs = RemoveDataWithMissingValues(trafficCSVs);

            var traffics = mapper.Map<List<TrafficCSV>, List<Traffic>>(trafficCSVs);

            if (traffics != null)
            {
                await UploadToDatabase(traffics);

                return new ProcessingResult
                {
                    AllRows = rowCount,
                    UploadedRows = traffics.Count(),
                };
            }        

            return new ProcessingResult
            {
                AllRows = 0,
                UploadedRows = 0,
            };
        }

        public List<TrafficCSV> GetTrafficCSVs(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using var csv = new CsvReader(reader, csvConfig);
                              
            return csv.GetRecords<TrafficCSV>().ToList();
        }

        public List<TrafficCSV> RemoveDataWithMissingValues(List<TrafficCSV> trafficCSVs)
        {
            trafficCSVs.RemoveAll(t =>
                string.IsNullOrWhiteSpace(t.County) ||
                string.IsNullOrWhiteSpace(t.Community) ||
                string.IsNullOrWhiteSpace(t.At) ||
                t.Latitude == 0 ||
                t.Longitude == 0 ||
                DateTime.ParseExact(t.LatestDate, "M/d/yyyy", CultureInfo.InvariantCulture) == DefaultDate
            );

            return trafficCSVs;
        }

        public async Task UploadToDatabase(List<Traffic> traffics)
        {
            using (var transaction = unitOfWork.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                await unitOfWork.GetRepository<Traffic>().AddRangeAsync(traffics);

                await unitOfWork.SaveAsync();

                await transaction.CommitAsync();
            }
        }
    }
}

