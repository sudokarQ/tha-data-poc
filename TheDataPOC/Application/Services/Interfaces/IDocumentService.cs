using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IDocumentService 
    {
        public Task<List<T>> GetData<T>(int pageNumber, int count) where T : class;

        public FileContentResult DownLoadData<T>(List<T> list, Type entityType) where T : class;
    }
}
