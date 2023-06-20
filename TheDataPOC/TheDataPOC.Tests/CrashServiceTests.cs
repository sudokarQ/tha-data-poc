namespace TheDataPOC.Tests
{
    using System.Data;
    using System.Linq.Expressions;

    using Application.DTOs;
    using Application.Services;

    using Domain.Enums;
    using Domain.Models;
    
    using Infrastructure.Repositories;
    using Infrastructure.UnitOfWork;
    
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore.Storage;
    
    using MockQueryable.Moq;
    
    using Moq;

    public class CrashServiceTests
    {
        private const string PrimaryFactor = "Sample";

        private const int Hour = 12;

        private readonly Mock<IUnitOfWork> unitOfWorkMock;

        private readonly CrashService crashService;

        public CrashServiceTests()
        {
            var transactionMock = new Mock<IDbContextTransaction>();

            unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(u => u.BeginTransaction(IsolationLevel.ReadUncommitted)).Returns(transactionMock.Object);
            unitOfWorkMock.Setup(u => u.BeginTransaction(IsolationLevel.ReadCommitted)).Returns(transactionMock.Object);

            unitOfWorkMock.Setup(u => u.GetRepository<Crash>()).Returns(new Mock<IGenericRepository<Crash>>().Object);

            crashService = new CrashService(unitOfWorkMock.Object);
        }

        [Fact]
        public async Task UpdateCrash_NonExistingCrash_ThrowsArgumentException()
        {
            // Arrange
            var crashs = new List<Crash>();

            unitOfWorkMock.Setup(u => u.GetRepository<Crash>()
            .Get(It.IsAny<Expression<Func<Crash, bool>>>()))
                .Returns(crashs.AsQueryable()
                .BuildMock());

            var updateDto = new CrashUpdateDto { PrimaryFactor = PrimaryFactor, Hour = Hour };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => crashService.UpdateCrashAsync(updateDto));
        }

        [Fact]
        public async Task DeleteCrash_NonExistingCrash_ThrowsArgumentException()
        {
            // Arrange
           
            var crash = new Crash { Id = Guid.NewGuid(), PrimaryFactor = PrimaryFactor, Hour = Hour };

            var crashs = new List<Crash>();

            unitOfWorkMock.Setup(u => u.GetRepository<Crash>().
                Get(It.IsAny<Expression<Func<Crash, bool>>>())).
                    Returns(crashs.AsQueryable().
                    BuildMock());

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => crashService.DeleteCrashAsync(crash.Id));
        }

        [Fact]
        public async Task DeleteCrash_ExistingCrash_CalledOnce()
        {
            // Arrange

            var crash = new Crash { Id = Guid.NewGuid(), PrimaryFactor = PrimaryFactor, Hour = Hour };

            var crashs = new List<Crash>();

            unitOfWorkMock.Setup(u => u.GetRepository<Crash>().
                Get(It.IsAny<Expression<Func<Crash, bool>>>())).
                    Returns(crashs.AsQueryable().
                    BuildMock());


            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => crashService.DeleteCrashAsync(crash.Id));
        }

        [Fact]
        public void InjuryTypeParser_WithFatalInjury_ReturnsFatal()
        {
            // Arrange
            var crashCSV = new CrashCSV
            {
                InjuryType = "Fatal"
            };
            
            // Act
            var result = crashService.InjuryTypeParser(crashCSV);

            // Assert
            Assert.Equal(InjuryType.Fatal, result);
        }

        [Fact]
        public void InjuryTypeParser_WithNonFatalInjury_ReturnsInjured()
        {
            // Arrange
            var crashCSV = new CrashCSV
            {
                InjuryType = "Non-fatal"
            };

            // Act
            var result = crashService.InjuryTypeParser(crashCSV);

            // Assert
            Assert.Equal(InjuryType.Injured, result);
        }

        [Fact]
        public void InjuryTypeParser_WithNoInjury_ReturnsNone()
        {
            // Arrange
            var crashCSV = new CrashCSV
            {
                InjuryType = "No injury/unknown"
            };

            // Act
            var result = crashService.InjuryTypeParser(crashCSV);

            // Assert
            Assert.Equal(InjuryType.None, result);
        }

        [Fact]
        public void InjuryTypeParser_WithUnknownInjury_ReturnsUnknown()
        {
            // Arrange
            var crashCSV = new CrashCSV
            {
                InjuryType = null
            };

            // Act
            var result = crashService.InjuryTypeParser(crashCSV);

            // Assert
            Assert.Equal(InjuryType.Unknown, result);
        }

        [Fact]
        public void TimeParser_WithValidTime_ReturnsParsedValue()
        {
            // Arrange
            var time = "10:30 AM";

            // Act
            var result = crashService.TimeParser(time);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public void TimeParser_WithInvalidTime_ReturnsNull()
        {
            // Arrange
            var time = "25:00 PM";

            // Act
            var result = crashService.TimeParser(time);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void DateParser_WithValidDateString_ReturnsParsedValue()
        {
            // Arrange
            var crashCSV = new CrashCSV
            {
                Date = "1/5/2022"
            };

            // Act
            var result = crashService.DateParser(crashCSV);

            // Assert
            Assert.Equal(new DateTime(2022, 1, 5), result);
        }

        [Fact]
        public void DateParser_WithInvalidDateString_ReturnsNull()
        {
            // Arrange
            var crashCSV = new CrashCSV
            {
                Date = "2022-01-05"
            };

            // Act
            var result = crashService.DateParser(crashCSV);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void DateParser_WithSeparateDateComponents_ReturnsParsedValue()
        {
            // Arrange
            var crashCSV = new CrashCSV
            {
                Year = "2022",
                Month = "1",
                Day = "5"
            };

            // Act
            var result = crashService.DateParser(crashCSV);

            // Assert
            Assert.Equal(new DateTime(2022, 1, 5), result);
        }

        [Fact]
        public void GetCrashCSVs_WithValidFile_ReturnsCrashCSVList()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var stream = GenerateFileStream("Master Record Number,Year,Month,Day,Weekend?,Hour,Collision Type,Injury Type,Primary Factor,Reported_Location,Latitude,Longitude\r\n902363382,2015,1,5,Weekday,0,2-Car,No injury/unknown,OTHER (DRIVER) - EXPLAIN IN NARRATIVE,1ST & FESS,39.15920668,-86.52587356\r\n902364268,2015,1,6,Weekday,1500,2-Car,No injury/unknown,FOLLOWING TOO CLOSELY,2ND & COLLEGE,39.16144,-86.534848");
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

            // Act
            var result = crashService.GetCrashCSVs(fileMock.Object);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void IsWeekend_WithWeekendDate_ReturnsTrue()
        {
            // Arrange
            var date = new DateTime(2022, 1, 8);

            // Act
            var result = crashService.IsWeekend(date);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsWeekend_WithWeekdayDate_ReturnsFalse()
        {
            // Arrange
            var date = new DateTime(2022, 1, 10);

            // Act
            var result = crashService.IsWeekend(date);

            // Assert
            Assert.False(result);
        }

        private Stream GenerateFileStream(string data)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(data);
            writer.Flush();

            stream.Position = 0;

            return stream;
        }
    }
}
