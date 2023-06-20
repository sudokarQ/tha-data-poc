namespace TheDataPOC.Tests
{
    using Application.Services;

    using Domain.Models;

    using Microsoft.AspNetCore.Http;

    using Moq;

    public class PedestrianServiceTests
    {
        [Fact]
        public void IsContainsDate_WhenDateColumnExists_ReturnsTrue()
        {
            // Arrange
            var file = CreateFormFile("Date,Column1,Column2", "Data");

            var service = new PedestrianService(null);

            // Act
            var result = service.IsContainsDate(file);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsContainsDate_WhenDateColumnDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var file = CreateFormFile("Column1,Column2,Column3", "Data");

            var service = new PedestrianService(null);

            // Act
            var result = service.IsContainsDate(file);

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

            var service = new PedestrianService(null);

            // Act
            var result = service.ProcessDefaultData(list);

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
            var service = new PedestrianService(null);

            // Act
            var result = service.ProcessDefaultData(list);

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
            var service = new PedestrianService(null);

            // Act
            var result = service.ProcessDataWithDate(list);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void DateParser_WhenInvalidDate_ReturnsNull()
        {
            // Arrange
            var day = "30";
            var year = "2023";
            var service = new PedestrianService(null);

            // Act
            var result = service.DateParser(day, year);

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
