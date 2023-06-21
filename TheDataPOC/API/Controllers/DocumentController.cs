namespace API.Controllers
{
    using System.ComponentModel.DataAnnotations;

    using Application.DTOs;

    using Application.Services.Interfaces;

    using Attributes;

    using Domain.Enums;
    using Domain.Models;

    using Microsoft.AspNetCore.Mvc;

    using Sieve.Models;
    using Sieve.Services;

    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private const int MaxRowsNumber = 200;

        private const int DefaultRowsNumber = 20;

        private const int DefaultPageNumber = 1;

        private readonly ISieveProcessor sieveProcessor;

        private readonly IUploadService uploadService;

        private readonly IDocumentService documentService;

        private readonly ICrashService crashService;

        private readonly ITrafficService trafficService;

        private readonly IPedestrianService pedestrianService;

        public DocumentController(
            ISieveProcessor sieveProcessor,
            IUploadService uploadService,
            IDocumentService documentService,
            ICrashService crashService,
            ITrafficService trafficService,
            IPedestrianService pedestrianService)
        {
            this.sieveProcessor = sieveProcessor;
            this.uploadService = uploadService;
            this.documentService = documentService;
            this.crashService = crashService;
            this.trafficService = trafficService;
            this.pedestrianService = pedestrianService;
        }

        [TableDataPaginationParametersFilter]
        [HttpGet("{tableName}/GetFilteredAndSorted")]
        public async Task<IActionResult> GetFilteredAndSortedData(
            string tableName,
            [FromQuery] SieveModel sieveModel,
            int pageNumber = DefaultPageNumber,
            int count = DefaultRowsNumber)
        {
            try
            {
                var tableNameEnum = Enum.Parse<TableName>(tableName, ignoreCase: true);

                switch (tableNameEnum)
                {
                    case TableName.Crash:
                        var crashes = await documentService.GetData<Crash>(pageNumber, count);
                        var crashesResult = sieveProcessor.Apply(sieveModel, crashes.AsQueryable());
                        return Ok(crashesResult);

                    case TableName.Pedestrian:
                        var pedestrians = await documentService.GetData<Pedestrian>(pageNumber, count);
                        var pedestriansResult = sieveProcessor.Apply(sieveModel, pedestrians.AsQueryable());
                        return Ok(pedestriansResult);

                    case TableName.Traffic:
                        var traffics = await documentService.GetData<Traffic>(pageNumber, count);
                        var trafficsResult = sieveProcessor.Apply(sieveModel, traffics.AsQueryable());
                        return Ok(trafficsResult);

                    default:
                        throw new ArgumentException("Type not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        [TableDataPaginationParametersFilter]
        [HttpGet("{tableName}")]
        public async Task<IActionResult> GetData(
            string tableName,
            [FromQuery] int pageNumber = DefaultPageNumber,
            int count = DefaultRowsNumber)
        {           
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

        [HttpDelete("{tableName}/{id}")]
        public async Task<IActionResult> DeleteData(string tableName, Guid id)
        {
            try
            {
                var tableNameEnum = Enum.Parse<TableName>(tableName, ignoreCase: true);

                switch (tableNameEnum)
                {
                    case TableName.Crash:
                        await crashService.DeleteCrashAsync(id);
                        break;

                    case TableName.Pedestrian:
                        await pedestrianService.DeletePedestrianAsync(id);
                        break;

                    case TableName.Traffic:
                        await trafficService.DeleteTrafficAsync(id);
                        break;

                    default:
                        throw new ArgumentException("Type not found");
                }

                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("crash")]
        public async Task<IActionResult> UpdateData(CrashUpdateDto dto)
        {
            try
            {
                var crash = await crashService.UpdateCrashAsync(dto);

                return Ok(crash);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("traffic")]
        public async Task<IActionResult> UpdateData(TrafficUpdateDto dto)
        {
            try
            {
                var traffic = await trafficService.UpdateTrafficAsync(dto);

                return Ok(traffic);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("pedestrian")]
        public async Task<IActionResult> UpdateData(PedestrianUpdateDto dto)
        {
            try
            {
                var pedestrian = await pedestrianService.UpdatePedestrianAsync(dto);

                return Ok(pedestrian);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
