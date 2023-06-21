namespace TheDataPOC.Tests
{
    using System.Data;
    using System.Linq.Expressions;

    using Application.DTOs;
    using Application.Services;

    using Domain.Models;
    
    using Infrastructure.Repositories;
    using Infrastructure.UnitOfWork;
    
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore.Storage;
    
    using MockQueryable.Moq;
    
    using Moq;

    public class PedestrianServiceTests
    {
        private const int SeventhAndParkCampus = 100;

        private const int BlineConventionCentre = 200;

        private readonly Mock<IUnitOfWork> unitOfWorkMock;

        private readonly PedestrianService pedestrianService;

        public PedestrianServiceTests()
        {
            var transactionMock = new Mock<IDbContextTransaction>();

            unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(u => u.BeginTransaction(IsolationLevel.ReadUncommitted)).Returns(transactionMock.Object);
            unitOfWorkMock.Setup(u => u.BeginTransaction(IsolationLevel.ReadCommitted)).Returns(transactionMock.Object);

            unitOfWorkMock.Setup(u => u.GetRepository<Pedestrian>()).Returns(new Mock<IGenericRepository<Pedestrian>>().Object);

            pedestrianService = new PedestrianService(unitOfWorkMock.Object);
        }

        [Fact]
        public async Task UpdatePedestrian_NonExistingPedestrian_ThrowsArgumentException()
        {
            // Arrange
            var pedestrians = new List<Pedestrian>();

            unitOfWorkMock.Setup(u => u.GetRepository<Pedestrian>().
            Get(It.IsAny<Expression<Func<Pedestrian, bool>>>()))
                .Returns(pedestrians.AsQueryable()
                .BuildMock());

            var updateDto = new PedestrianUpdateDto { SeventhAndParkCampus = SeventhAndParkCampus, BlineConventionCentre = BlineConventionCentre };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => pedestrianService.UpdatePedestrianAsync(updateDto));
        }

        [Fact]
        public async Task DeletePedestrian_NonExistingPedestrian_ThrowsArgumentException()
        {
            // Arrange

            var pedestrian = new Pedestrian { Id = Guid.NewGuid(), SeventhAndParkCampus = SeventhAndParkCampus, BlineConventionCentre = BlineConventionCentre };

            var pedestrians = new List<Pedestrian>();

            unitOfWorkMock.Setup(u => u.GetRepository<Pedestrian>().
                Get(It.IsAny<Expression<Func<Pedestrian, bool>>>()))
                    .Returns(pedestrians.AsQueryable()
                    .BuildMock());

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => pedestrianService.DeletePedestrianAsync(pedestrian.Id));
        }

        [Fact]
        public async Task DeletePedestrian_ExistingPedestrian_CalledOnce()
        {
            // Arrange

            var pedestrian = new Pedestrian { Id = Guid.NewGuid(), SeventhAndParkCampus = SeventhAndParkCampus, BlineConventionCentre = BlineConventionCentre };

            var pedestrians = new List<Pedestrian>();

            unitOfWorkMock.Setup(u => u.GetRepository<Pedestrian>().
                Get(It.IsAny<Expression<Func<Pedestrian, bool>>>()))
                    .Returns(pedestrians.AsQueryable()
                    .BuildMock());

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => pedestrianService.DeletePedestrianAsync(pedestrian.Id));
        }

        [Fact]
        public void IsContainsDate_WhenDateColumnExists_ReturnsTrue()
        {
            // Arrange
            var file = CreateFormFile("Date,Column1,Column2", "Data");

            // Act
            var result = pedestrianService.IsContainsDate(file);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsContainsDate_WhenDateColumnDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var file = CreateFormFile("Column1,Column2,Column3", "Data");

            // Act
            var result = pedestrianService.IsContainsDate(file);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ProcessDefaultData_WhenValidListProvided_ReturnsListOfPedestrian()
        {
            // Arrange
            var list = new List<PedestrianCSV>
            {
                new PedestrianCSV { Date = "01-01-2019 00:00:00", SeventhAndParkCampus = 1, BlineConventionCentre = 2, JordanAndSeventh = 3, NCollegeAndRailRoad = 4, SWalnutAndWylie = 5 }
            };

            // Act
            var result = pedestrianService.ProcessDefaultData(list);

            // Assert
            Assert.NotNull(result);

            Assert.Single(result);
            
            Assert.Equal(new DateTime(2019, 1, 1), result[0].Date);
            Assert.Equal(1, result[0].SeventhAndParkCampus);
            Assert.Equal(2, result[0].BlineConventionCentre);
            Assert.Equal(3, result[0].JordanAndSeventh);
            Assert.Equal(4, result[0].NCollegeAndRailRoad);
            Assert.Equal(5, result[0].SWalnutAndWylie);
        }

        [Fact]
        public void ProcessDefaultData_WhenInvalidDateFormat_ReturnsEmptyList()
        {
            // Arrange
            var list = new List<PedestrianCSV>
            {
                new PedestrianCSV { Date = "2023-06-16T08:30:00", SeventhAndParkCampus = 1, BlineConventionCentre = 2, JordanAndSeventh = 3, NCollegeAndRailRoad = 4, SWalnutAndWylie = 5 }
            };

            // Act
            var result = pedestrianService.ProcessDefaultData(list);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ProcessDataWithDate_WhenInvalidDate_ReturnsEmptyList()
        {
            // Arrange
            var list = new List<PedestrianCSVDate>
            {
                new PedestrianCSVDate { Day = "June 16", Year = "2023", SeventhAndParkCampus = 1, BlineConventionCentre = 2, JordanAndSeventh = 3, NCollegeAndRailRoad = 4, SWalnutAndWylie = 5 }
            };

            // Act
            var result = pedestrianService.ProcessDataWithDate(list);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void DateParser_WhenInvalidDate_ReturnsNull()
        {
            // Arrange
            var day = "30";
            var year = "2023";

            // Act
            var result = pedestrianService.DateParser(day, year);

            // Assert
            Assert.Null(result);
        }

        private static IFormFile CreateFormFile(string header, string data)
        {
            var fileMock = new Mock<IFormFile>();
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write($"{header}\n{data}");
            writer.Flush();

            stream.Position = 0;
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

            return fileMock.Object;
        }
    }

}
