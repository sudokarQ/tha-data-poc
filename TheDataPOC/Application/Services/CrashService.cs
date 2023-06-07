namespace Application.Services
{
    using Application.CSVHelper;
    using Application.Services.Interfaces;
    using CsvHelper;
    using Domain.Enums;
    using Domain.Models;
    using Infrastructure.UnitOfWork;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Data;
    using System.Globalization;

    public class CrashService : ICrashService
    {
        private readonly IUnitOfWork unitOfWork;

        public CrashService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<(int, int)> DataProcessing(IFormFile file)
        {
            var list = GetCrashCSVs(file);

            var totalRows = list.Count;

            var result = new List<Crash>();

            list.RemoveAll(c =>
                string.IsNullOrWhiteSpace(c.PrimaryFactor) ||
                string.IsNullOrWhiteSpace(c.Time) ||
                string.IsNullOrWhiteSpace(c.Latitude) ||
                string.IsNullOrWhiteSpace(c.Longitude)
            );

            foreach (var item in list)
            {
                var date = DateParser(item);

                var injType = InjuryTypeParser(item);

                if (date is null || injType == InjuryType.Unknown)
                    continue;

                var temp = new Crash
                {
                    Id = Guid.NewGuid(),
                    PrimaryFactor = item.PrimaryFactor,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    Date = date,
                    Hour = TimeParser(item.Time),
                    InjuryType = injType,
                    Day = date.Value.Day,
                    Month = date.Value.Month,
                    Year = date.Value.Year,
                    IsWeekend = IsWeekend(date.Value),
                };

                result.Add(temp);
            }

            var uploadedRows = result.Count();

            await UploadToDatabase(result);

            return (totalRows, uploadedRows);
        }

        public async Task UploadToDatabase(List<Crash> crashes)
        {
            using (var transaction = unitOfWork.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                await unitOfWork.GetRepository<Crash>().AddRangeAsync(crashes);

                await unitOfWork.SaveAsync();

                await transaction.CommitAsync();
            }
        }

        public InjuryType InjuryTypeParser(CrashCSV crashCSV)
        {
            var dead = int.TryParse(crashCSV.Dead, out var numberDead);

            if (crashCSV.InjuryType == InjuryType.Fatal.ToString() || numberDead > 0 || crashCSV.NumberDead == "true")
            {
                return InjuryType.Fatal;
            }

            if (crashCSV.InjuryType == "No injury/unknown" || crashCSV.Inj == "0" || crashCSV.NumberInjured == "0")
            {
                return InjuryType.None;

            }

            if (crashCSV.InjuryType is not null || crashCSV.Inj is not null || crashCSV.NumberInjured is not null)
            {
                return InjuryType.Injured;
            }

            return InjuryType.Unknown;
        }


        public int? TimeParser(string time)
        {
            time = time.Trim();

            if (time.Length <= 3)
            {
                return Convert.ToInt32(time[0]);
            }

            if (time.Length == 4)
            {
                return Convert.ToInt32(time[0..2]);
            }

            if (time.Contains(':') && (time.Contains("PM") || time.Contains("AM")))
            {
                var result = Convert.ToInt32(time.Split(':')[0]);

                if (result > 12)
                {
                    return null;
                }

                if (time.EndsWith("AM"))
                {
                    return result;
                }
                else
                {
                    return result == 12 ? 23 : result + 12;
                }

            }

            return null;
        }

        public DateTime? DateParser(CrashCSV crashCSV)
        {
            DateTime? result = null;

            if (crashCSV.Date is not null)
            {
                try
                {
                    result = DateTime.ParseExact(crashCSV.Date, "M/d/yyyy", CultureInfo.InvariantCulture);
                }
                catch
                {
                    return result;
                }

            }

            if (!string.IsNullOrWhiteSpace(crashCSV.Year) && !string.IsNullOrWhiteSpace(crashCSV.Month) && !string.IsNullOrWhiteSpace(crashCSV.Day))
            {
                result = new DateTime(int.Parse(crashCSV.Year), int.Parse(crashCSV.Month), int.Parse(crashCSV.Day));
            }

            return result;
        }

        public List<CrashCSV> GetCrashCSVs(IFormFile file)
        {
            List<CrashCSV> crashes;

            using (var reader = new StreamReader(file.OpenReadStream()))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<CrashMap>();
                crashes = csv.GetRecords<CrashCSV>().ToList();
            }

            return crashes;
        }

        public bool IsWeekend(DateTime date)
            => (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday);
    }
}

