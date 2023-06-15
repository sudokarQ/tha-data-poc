namespace API.Controllers
{
    using Application.Services.Interfaces;

    using Domain.Enums;
    using Domain.Models;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private const int MaxRowsNumber = 200;

        private const int DefaultRowsNumber = 20;

        private const int DefaultPageNumber = 1;

        private readonly IUploadService uploadService;

        private readonly IDocumentService documentService;

        public DocumentController(IUploadService uploadService, IDocumentService documentService)
        {
            this.uploadService = uploadService;
            this.documentService = documentService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadData(IFormFile file)
        {
            try
            {
                var result = await uploadService.UploadFileAsync(file);

                return Ok($"Total rows:{result.AllRows}, uploaded rows: {result.UploadedRows}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{tableName}")]
        public async Task<IActionResult> GetData(string tableName, [FromQuery] int pageNumber = DefaultPageNumber, int count = DefaultRowsNumber)
        {
            if (pageNumber <= 0 || count <= 0 || string.IsNullOrWhiteSpace(tableName))
            {
                return BadRequest("Invalid parameters. Please check the properties.");
            }

            if (count > MaxRowsNumber)
            {
                return BadRequest("Maximum row count exceeded. Please limit the count to 200 or fewer.");
            }

            try
            {
                var tableNameEnum = Enum.Parse<TableName>(tableName, ignoreCase: true);

                switch (tableNameEnum)
                {
                    case TableName.Crash:
                        var crashData = await documentService.GetData<Crash>(pageNumber, count);
                        return Ok(crashData);

                    case TableName.Pedestrian:
                        var pedestrianData = await documentService.GetData<Pedestrian>(pageNumber, count);
                        return Ok(pedestrianData);

                    case TableName.Traffic:
                        var trafficData = await documentService.GetData<Traffic>(pageNumber, count);
                        return Ok(trafficData);

                    default:
                        throw new ArgumentException("Type not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{tableName}/download")]
        public async Task<IActionResult> DownloadData(string tableName, int count = DefaultRowsNumber)
        {
            int pageNumber = DefaultPageNumber;

            if (count <= 0 || string.IsNullOrWhiteSpace(tableName))
            {
                return BadRequest("Invalid parameters. Please check the properties.");
            }

            try
            {
                var tableNameEnum = Enum.Parse<TableName>(tableName, ignoreCase: true);

                switch (tableNameEnum)
                {
                    case TableName.Crash:
                        var crashData = await documentService.GetData<Crash>(pageNumber, count);
                        return documentService.DownLoadData(crashData, typeof(Crash));

                    case TableName.Pedestrian:
                        var pedestrianData = await documentService.GetData<Pedestrian>(pageNumber, count);
                        return documentService.DownLoadData(pedestrianData, typeof(Pedestrian));

                    case TableName.Traffic:
                        var trafficData = await documentService.GetData<Traffic>(pageNumber, count);
                        return documentService.DownLoadData(trafficData, typeof(Traffic));

                    default:
                        throw new ArgumentException("Type not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
