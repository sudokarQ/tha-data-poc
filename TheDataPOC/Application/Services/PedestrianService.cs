namespace Application.Services
{
    using System;
    using System.Data;
    using System.Globalization;

    using Application.CSVHelper;

    using CsvHelper;
    
    using Domain.Models;
    
    using Infrastructure.UnitOfWork;

    using Interfaces;
    
    using Microsoft.AspNetCore.Http;

    public class PedestrianService : IPedestrianService
    {
        private readonly IUnitOfWork unitOfWork;

        public PedestrianService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ProcessingResult> DataProcessing(IFormFile file)
        {
            List<Pedestrian> result;

            int totalRows;

            if (IsContainsDate(file))
            {
                var temp = GetPedestrianCSVDate(file);

                totalRows = temp.Count();

                result = ProcessDataWithDate(temp);
            }
            else
            {
                var temp = GetPedestrianCSV(file);

                totalRows = temp.Count();

                result = ProcessDefaultData(temp);
            }

            var uploadedRows = result.Count();

            if (uploadedRows != 0)
            {
                await UploadToDatabase(result);
            }            

            return new ProcessingResult { AllRows = totalRows, UploadedRows = uploadedRows, };
        }

        public async Task UploadToDatabase(List<Pedestrian> pedestrians)
        {
            using (var transaction = unitOfWork.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                await unitOfWork.GetRepository<Pedestrian>().AddRangeAsync(pedestrians);

                await unitOfWork.SaveAsync();

                await transaction.CommitAsync();
            }
        }

        public DateTime? DateParser(string day, string year)
        {
            string dateTimeString = $"{day.Trim()} {year.Trim()}";

            DateTime parsedDateTime;

            return DateTime.TryParseExact(dateTimeString,
                "MMM d yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out parsedDateTime)
                ? parsedDateTime
                : (DateTime?)null;
        }



        public bool IsContainsDate(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {

                csv.Read();
                csv.ReadHeader();

                string[] headerRow = csv.Context.Reader.HeaderRecord;
                csv.Read();

                var firstDataRow = csv.Parser.RawRecord;
                
                if (headerRow.Contains("Date") && !firstDataRow.Contains("\""))
                {
                    csv.Read();
                    return true;
                }

                return false;
            }
        }

        public List<PedestrianCSV> GetPedestrianCSV(IFormFile file)
        {
            List<PedestrianCSV> pedestrians;

            using (var reader = new StreamReader(file.OpenReadStream()))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<PedestrianMap>();
                pedestrians = csv.GetRecords<PedestrianCSV>().ToList();
            }

            return pedestrians;
        }

        public List<PedestrianCSVDate> GetPedestrianCSVDate(IFormFile file)
        {
            List<PedestrianCSVDate> pedestrians;

            using (var reader = new StreamReader(file.OpenReadStream()))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                pedestrians = csv.GetRecords<PedestrianCSVDate>().ToList();
            }

            return pedestrians;
        }

        public List<Pedestrian> ProcessDataWithDate(List<PedestrianCSVDate> list)
        {
            var result = new List<Pedestrian>();

            list.RemoveAll(c =>
                c.BlineConventionCentre is null ||
                c.SeventhAndParkCampus is null ||
                c.SWalnutAndWylie is null ||
                c.JordanAndSeventh is null ||
                c.NCollegeAndRailRoad is null
                );

            foreach (var item in list)
            {
                var date = DateParser(item.Day, item.Year);

                if (date is null)
                    continue;

                var temp = new Pedestrian
                {
                    Id = Guid.NewGuid(),
                    Date = date,
                    SeventhAndParkCampus = (int)item.SeventhAndParkCampus,
                    BlineConventionCentre = (int)item.BlineConventionCentre,
                    JordanAndSeventh = (int)item.JordanAndSeventh,
                    NCollegeAndRailRoad = (int)item.NCollegeAndRailRoad,
                    SWalnutAndWylie = (int)item.SWalnutAndWylie,
                };

                result.Add(temp);
            }

            return result;
        }

        public List<Pedestrian> ProcessDefaultData(List<PedestrianCSV> list)
        {
            var result = new List<Pedestrian>();

            list.RemoveAll(c =>
                c.BlineConventionCentre is null ||
                c.SeventhAndParkCampus is null ||
                c.SWalnutAndWylie is null ||
                c.JordanAndSeventh is null ||
                c.NCollegeAndRailRoad is null
                );

            string[] formats = { "ddd, MMM d, yyyy", "dddd, MMM d, yyyy", "dd-MM-yyyy HH:mm:ss" };

            foreach (var item in list)
            {
                if (!DateTime.TryParseExact(item.Date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
                {
                    continue;
                }

                var temp = new Pedestrian
                {
                    Id = Guid.NewGuid(),
                    Date = parsedDateTime,
                    SeventhAndParkCampus = (int)item.SeventhAndParkCampus,
                    BlineConventionCentre = (int)item.BlineConventionCentre,
                    JordanAndSeventh = (int)item.JordanAndSeventh,
                    NCollegeAndRailRoad = (int)item.NCollegeAndRailRoad,
                    SWalnutAndWylie = (int)item.SWalnutAndWylie,
                };

                result.Add(temp);
            }

            return result;
        }
    }
}

