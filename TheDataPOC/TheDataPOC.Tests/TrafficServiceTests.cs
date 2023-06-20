namespace TheDataPOC.Tests
{
    using System.Data;
    using System.Linq.Expressions;
    using System.Text;

    using Application.AutoMapping;
    using Application.DTOs;
    using Application.Services;

    using AutoMapper;

    using Domain.Models;
    
    using Infrastructure.Repositories;
    using Infrastructure.UnitOfWork;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore.Storage;
    
    using MockQueryable.Moq;
    
    using Moq;

    public class TrafficServiceTests
    {
        private readonly string Country = "Test";
        
        private readonly string Community = "Test";

        private readonly Mock<IUnitOfWork> unitOfWorkMock;

        private readonly TrafficService trafficService;

        private readonly IMapper mapper;

        public TrafficServiceTests()
        {
            var transactionMock = new Mock<IDbContextTransaction>();

            this.unitOfWorkMock = new Mock<IUnitOfWork>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TrafficProfile());
            });

            this.mapper = mapperConfig.CreateMapper();
           
            unitOfWorkMock.Setup(u => u.BeginTransaction(IsolationLevel.ReadUncommitted)).Returns(transactionMock.Object);

            unitOfWorkMock.Setup(u => u.GetRepository<Traffic>()).Returns(new Mock<IGenericRepository<Traffic>>().Object);

            trafficService = new TrafficService(unitOfWorkMock.Object, mapper); 
        }

        [Fact]
        public async Task UpdateTraffic_NonExistingTraffic_ThrowsArgumentException()
        {
            // Arrange
            var traffics = new List<Traffic>();

            unitOfWorkMock.Setup(u => u.GetRepository<Traffic>()
                .Get(It.IsAny<Expression<Func<Traffic, bool>>>()))
                    .Returns(traffics.AsQueryable()
                    .BuildMock());

            var updateDto = new TrafficUpdateDto { County = Country, Community = Community };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => trafficService.UpdateTrafficAsync(updateDto));
        }

        [Fact]
        public async Task DeleteTraffic_NonExistingTraffic_ThrowsArgumentException()
        {
            // Arrange

            var traffic = new Traffic { TrafficId = Guid.NewGuid(), County = Country, Community = Community };

            var traffics = new List<Traffic>();

            unitOfWorkMock.Setup(u => u.GetRepository<Traffic>()
                .Get(It.IsAny<Expression<Func<Traffic, bool>>>()))
                    .Returns(traffics.AsQueryable()
                    .BuildMock());

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => trafficService.DeleteTrafficAsync(traffic.TrafficId));
        }

        [Fact]
        public async Task DeleteTraffic_ExistingTraffic_CalledOnce()
        {
            // Arrange

            var traffic = new Traffic { TrafficId = Guid.NewGuid(), County = Country, Community = Community };

            var traffics = new List<Traffic>();

            unitOfWorkMock.Setup(u => u.GetRepository<Traffic>()
                .Get(It.IsAny<Expression<Func<Traffic, bool>>>()))
                    .Returns(traffics.AsQueryable()
                    .BuildMock());

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => trafficService.DeleteTrafficAsync(traffic.TrafficId));
        }

        [Fact]
        public async void ProcessEmptyFilePositive()
        {
            // Arrange
            var trafficService = new TrafficService(unitOfWorkMock.Object, mapper);

            FormFile file;

            using (var stream = File.Create("traffic-count.csv"))
            {
                file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                };

                // Act
                var result = await trafficService.SaveDataAsync(file);

                //Assert
                Assert.Equal((0,0), (result.AllRows, result.UploadedRows));
            }
        }

        [Fact]
        public async void ProcessEmptyFileNegative()
        {
            // Arrange
            var trafficService = new TrafficService(unitOfWorkMock.Object, mapper);

            FormFile file;

            using (var stream = File.Create("traffic-count.csv"))
            {
                file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                };

                // Act
                var result = await trafficService.SaveDataAsync(file);

                //Assert
                Assert.NotEqual((10, 15), (result.AllRows, result.UploadedRows));
            }
        }

        [Fact]
        public async void ProcessFileWithMissingDate()
        {
            // Arrange            
            var trafficService = new TrafficService(unitOfWorkMock.Object, mapper);

            var fileContent = "Loc ID,County,Community,Functional Class,Rural Urban,On,From,To,Approach,At,Dir,Directions,Category,LRS ID,LRS Loc Pt,Latitude,Longitude,Latest,Latest Date\n" +
                              "l383,Monroe,Bloomington,-,,Leonard Springs Road,,,,From W. Heatherwood Ln. to W. Woodhaven Dr.,2 - WAY,NB / SB,,,,39.1275323,-86.58257118,3092,12/12/2019\n" +
                              "l383,Monroe,Bloomington,-,,Leonard Springs Road,,,,From W. Heatherwood Ln. to W. Woodhaven Dr.,2 - WAY,NB / SB,,,,39.1275323,-86.58257118,3092,\n";

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
            fileMock.Setup(x => x.FileName).Returns("traffic-count.csv");

            // Act
            var result = await trafficService.SaveDataAsync(fileMock.Object);

            // Assert
            Assert.Equal((2, 1), (result.AllRows, result.UploadedRows));
        }

        [Fact]
        public async void ProcessFileWithWrongLatitude()
        {
            // Arrange            
            var trafficService = new TrafficService(unitOfWorkMock.Object, mapper);

            var fileContent = "Loc ID,County,Community,Functional Class,Rural Urban,On,From,To,Approach,At,Dir,Directions,Category,LRS ID,LRS Loc Pt,Latitude,Longitude,Latest,Latest Date\n" +
                              "l383,Monroe,Bloomington,-,,Leonard Springs Road,,,,From W. Heatherwood Ln. to W. Woodhaven Dr.,2 - WAY,NB / SB,,,,0,-86.58257118,3092,12/12/2019\n" +
                              "l383,Monroe,Bloomington,-,,Leonard Springs Road,,,,From W. Heatherwood Ln. to W. Woodhaven Dr.,2 - WAY,NB / SB,,,,39.1275323,-86.58257118,3092,12/12/2019\n";

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
            fileMock.Setup(x => x.FileName).Returns("traffic-count.csv");

            // Act
            var result = await trafficService.SaveDataAsync(fileMock.Object);

            // Assert
            Assert.Equal((2, 1), (result.AllRows, result.UploadedRows));
        }

        [Fact]
        public async void ProcessFileWithWrongLongitude()
        {
            // Arrange            
            var trafficService = new TrafficService(unitOfWorkMock.Object, mapper);

            var fileContent = "Loc ID,County,Community,Functional Class,Rural Urban,On,From,To,Approach,At,Dir,Directions,Category,LRS ID,LRS Loc Pt,Latitude,Longitude,Latest,Latest Date\n" +
                              "l383,Monroe,Bloomington,-,,Leonard Springs Road,,,,From W. Heatherwood Ln. to W. Woodhaven Dr.,2 - WAY,NB / SB,,,,39.1275323,0,3092,12/12/2019\n" +
                              "l383,Monroe,Bloomington,-,,Leonard Springs Road,,,,From W. Heatherwood Ln. to W. Woodhaven Dr.,2 - WAY,NB / SB,,,,39.1275323,-86.58257118,3092,12/12/2019\n";

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
            fileMock.Setup(x => x.FileName).Returns("traffic-count.csv");

            // Act
            var result = await trafficService.SaveDataAsync(fileMock.Object);

            // Assert
            Assert.Equal((2, 1), (result.AllRows, result.UploadedRows));
        }

        [Fact]
        public async void ProcessFileWithMissingCounty()
        {
            // Arrange            
            var trafficService = new TrafficService(unitOfWorkMock.Object, mapper);

            var fileContent = "Loc ID,County,Community,Functional Class,Rural Urban,On,From,To,Approach,At,Dir,Directions,Category,LRS ID,LRS Loc Pt,Latitude,Longitude,Latest,Latest Date\n" +
                              "l383,,Bloomington,-,,Leonard Springs Road,,,,From W. Heatherwood Ln. to W. Woodhaven Dr.,2 - WAY,NB / SB,,,,39.1275323,-86.58257118,3092,12/12/2019\n" +
                              "l383,Monroe,Bloomington,-,,Leonard Springs Road,,,,From W. Heatherwood Ln. to W. Woodhaven Dr.,2 - WAY,NB / SB,,,,39.1275323,-86.58257118,3092,12/12/2019\n";

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
            fileMock.Setup(x => x.FileName).Returns("traffic-count.csv");

            // Act
            var result = await trafficService.SaveDataAsync(fileMock.Object);

            // Assert
            Assert.Equal((2, 1), (result.AllRows, result.UploadedRows));
        }

        [Fact]
        public async void ProcessFileWithMissingCommunity()
        {
            // Arrange            
            var trafficService = new TrafficService(unitOfWorkMock.Object, mapper);

            var fileContent = "Loc ID,County,Community,Functional Class,Rural Urban,On,From,To,Approach,At,Dir,Directions,Category,LRS ID,LRS Loc Pt,Latitude,Longitude,Latest,Latest Date\n" +
                              "l383,Monroe,,-,,Leonard Springs Road,,,,From W. Heatherwood Ln. to W. Woodhaven Dr.,2 - WAY,NB / SB,,,,39.1275323,-86.58257118,3092,12/12/2019\n" +
                              "l383,Monroe,Bloomington,-,,Leonard Springs Road,,,,From W. Heatherwood Ln. to W. Woodhaven Dr.,2 - WAY,NB / SB,,,,39.1275323,-86.58257118,3092,12/12/2019\n";

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
            fileMock.Setup(x => x.FileName).Returns("traffic-count.csv");

            // Act
            var result = await trafficService.SaveDataAsync(fileMock.Object);

            // Assert
            Assert.Equal((2, 1), (result.AllRows, result.UploadedRows));
        }

        [Fact]
        public async void ProcessFileWithMissingAtColumnValue()
        {
            // Arrange            
            var trafficService = new TrafficService(unitOfWorkMock.Object, mapper);

            var fileContent = "Loc ID,County,Community,Functional Class,Rural Urban,On,From,To,Approach,At,Dir,Directions,Category,LRS ID,LRS Loc Pt,Latitude,Longitude,Latest,Latest Date\n" +
                              "l383,Monroe,Bloomington,-,,Leonard Springs Road,,,,,2 - WAY,NB / SB,,,,39.1275323,-86.58257118,3092,12/12/2019\n" +
                              "l383,Monroe,Bloomington,-,,Leonard Springs Road,,,,From W. Heatherwood Ln. to W. Woodhaven Dr.,2 - WAY,NB / SB,,,,39.1275323,-86.58257118,3092,12/12/2019\n";

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

            var fileMock = new Mock<IFormFile>();

            fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
            fileMock.Setup(x => x.FileName).Returns("traffic-count.csv");

            // Act
            var result = await trafficService.SaveDataAsync(fileMock.Object);

            // Assert
            Assert.Equal((2, 1), (result.AllRows, result.UploadedRows));
        }
    }
}

