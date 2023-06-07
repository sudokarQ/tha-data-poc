namespace TheDataPOC.Tests
{
    using System.IO;
    using System.Threading.Tasks;

    using Application.Services;
    using Application.Services.Interfaces;

    using Infrastructure.UnitOfWork;
    
    using Microsoft.AspNetCore.Http;

    using Moq;

    public class UploadServiceTests
	{
        private readonly ICrashService crashService;

        private readonly Mock<IUnitOfWork> unitOfWorkMock;


        public UploadServiceTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();

            ICrashService crashService = new CrashService(unitOfWorkMock.Object);

        }

        [Fact]
        public async Task UploadCSVFilesNegative()
        {
            // Arrange
            UploadService uploadService = new UploadService(crashService);

            FormFile file;

            using (var stream = File.Create("placeholder.txt"))
            {
                file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                };
            }

            // Act
            async Task Act() => await uploadService.UploadFileAsync(file);

            // Assert
            var exception = await Assert.ThrowsAsync<Exception>(Act);
            Assert.Equal("You can upload only .csv files", exception.Message);
        }

        [Fact]
        public async Task UploadCSVFilesPositive()
        {
            // Arrange
            UploadService uploadService = new UploadService(crashService);

            FormFile file;

            using (var stream = File.Create("placeholder.csv"))
            {
                file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                };
            }

            // Act
            var result = await uploadService.UploadFileAsync(file);

            // Assert
            Assert.Equal((0, 0), result);
        }

        [Fact]
        public async Task UploadServiceResultNotNullPositive()
        {
            // Arrange
            UploadService uploadService = new UploadService(crashService);

            FormFile file;

            using (var stream = File.Create("placeholder.csv"))
            {
                file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                };
            }

            // Act
            var result = await uploadService.UploadFileAsync(file);

            // Assert
            Assert.NotNull(result);
        }
    }
}

