﻿namespace Application.Services
{
    using System.Data;
    using System.Globalization;
    
    using AutoMapper;

    using CsvHelper;
    using CsvHelper.Configuration;
    
    using Domain.Models;
    
    using DTOs;
    
    using Infrastructure.UnitOfWork;
    
    using Interfaces;
    
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    
    
    using Validation;

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

        public async Task<Traffic> UpdateTrafficAsync(TrafficUpdateDto dto)
        {
            var traffic = await unitOfWork.GetRepository<Traffic>().Get(r => r.TrafficId == dto.TrafficId).FirstOrDefaultAsync();

            if (traffic is null)
            {
                throw new ArgumentException("Traffic not found");
            }

            if (!ValidationHelper.Validate(dto))
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("Incorrect Latitude or Longtitude");
            }

            using (var transaction = unitOfWork.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                traffic.County = dto.County ?? traffic.County;
                traffic.Community = dto.Community ?? traffic.Community;
                traffic.Street = dto.Street ?? traffic.Street;
                traffic.At = dto.At ?? traffic.At;
                traffic.Directory = dto.Directory ?? traffic.Directory;
                traffic.Directions = dto.Directions ?? traffic.Directions;
                traffic.Latitude = dto.Latitude ?? traffic.Latitude;
                traffic.Longitude = dto.Longitude ?? traffic.Longitude;

                unitOfWork.GetRepository<Traffic>().Update(traffic);

                await unitOfWork.SaveAsync();

                await transaction.CommitAsync();
            }

            return traffic;
        }

        public async Task DeleteTrafficAsync(Guid trafficId)
        {
            var traffic = await unitOfWork.GetRepository<Traffic>().Get(r => r.TrafficId == trafficId).FirstOrDefaultAsync();

            if (traffic is null)
            {
                throw new ArgumentException("Traffic not found");
            }

            using (var transaction = unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                unitOfWork.GetRepository<Traffic>().Remove(traffic);

                await unitOfWork.SaveAsync();

                await transaction.CommitAsync();
            }
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

