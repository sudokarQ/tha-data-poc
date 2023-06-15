namespace Application.Services
{
    using Application.Services.Interfaces;
    
    using CsvHelper;
    
    using Infrastructure.UnitOfWork;
    
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork unitOfWork;

        public DocumentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<T>> GetData<T>(int pageNumber, int count) where T : class
        {
            return await unitOfWork.GetRepository<T>()
                .Get()
                .Skip((pageNumber - 1) * count)
                .Take(count)
                .ToListAsync();
        }

        public FileContentResult DownLoadData<T>(List<T> list, Type entityType) where T : class
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteHeader<T>();
                    csvWriter.NextRecord();

                    csvWriter.WriteRecords(list);

                    writer.Flush();
                    stream.Position = 0;

                    var fileContentResult = new FileContentResult(stream.ToArray(), "text/csv")
                    {
                        FileDownloadName = $"{entityType.Name}.csv"
                    };

                    return fileContentResult;
                }
            }
        }
    }
}
