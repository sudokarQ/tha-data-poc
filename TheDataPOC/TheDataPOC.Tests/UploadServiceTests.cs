namespace TheDataPOC.Tests
{
    using Application.Services;
    using Application.Services.Interfaces;

    using AutoMapper;

    using Domain.Models;
   
    using Infrastructure.UnitOfWork;
    
    using Microsoft.AspNetCore.Http;
    
    using Moq;

    public class UploadServiceTests
    {
        private readonly ICrashService crashService;

        private readonly ITrafficService trafficService;

        private readonly Mock<IUnitOfWork> unitOfWorkMock;

        private readonly Mock<IMapper> mapperMock;

        public UploadServiceTests()
        {
            this.unitOfWorkMock = new Mock<IUnitOfWork>();

            this.mapperMock = new Mock<IMapper>();

            this.crashService = new CrashService(unitOfWorkMock.Object);

            this.trafficService = new TrafficService(unitOfWorkMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task UploadCSVFilesNegative()
        {
            // Arrange
            var uploadService = new UploadService(crashService, trafficService);

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
            var uploadService = new UploadService(crashService, trafficService);

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
            Assert.Equal((0,0), (result.UploadedRows, result.AllRows));
        }

        [Fact]
        public async Task ProcessTrafficFiles()
        {
            // Arrange
            var uploadService = new UploadService(crashService, trafficService);

            FormFile file;

            using (var stream = File.Create("traffic-count-2001.csv"))
            {
                file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                };

                // Act
                var result = await uploadService.UploadFileAsync(file);

                // Assert
                Assert.Equal((0, 0), (result.AllRows, result.UploadedRows));
            }            
        }

        [Fact]
        public async Task ProcessCrashFiles()
        {
            // Arrange
            var uploadService = new UploadService(crashService, trafficService);

            FormFile file;

            using (var stream = File.Create("crash-2001.csv"))
            {
                file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                };

                // Act
                var result = await uploadService.UploadFileAsync(file);

                // Assert
                Assert.Equal((0, 0), (result.AllRows, result.UploadedRows));                
            }
        }
    }
}
