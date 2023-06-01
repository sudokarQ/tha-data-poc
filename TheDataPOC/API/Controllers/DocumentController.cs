namespace API.Controllers
{
    using Application.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IUploadService uploadService;

        public DocumentController(IUploadService uploadService)
        {
            this.uploadService = uploadService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadData(IFormFile file)
        {
            try
            {
                var result = await uploadService.UploadFileAsync(file);

                return Ok($"Total rows:{result}, uploaded rows: {result}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

