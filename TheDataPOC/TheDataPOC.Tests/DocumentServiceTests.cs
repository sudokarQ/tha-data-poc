namespace Application.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    
    using Domain.Models;
    
    using Infrastructure.Repositories;
    using Infrastructure.UnitOfWork;
    
    using Microsoft.AspNetCore.Mvc;
    
    using MockQueryable.Moq;
    
    using Moq;
    
    using Xunit;

    public class DocumentServiceTests
    {
        private Mock<IUnitOfWork> unitOfWorkMock;

        private Mock<IGenericRepository<Crash>> repositoryMock;

        private List<Crash> data = new List<Crash>
            {
                new Crash { Id = Guid.NewGuid(), Year = 2021, Month = 6, Day = 1 },
                new Crash { Id = Guid.NewGuid(), Year = 2021, Month = 6, Day = 2 },
                new Crash { Id = Guid.NewGuid(), Year = 2021, Month = 6, Day = 3 }
            };

        public DocumentServiceTests()
        {
            repositoryMock = new Mock<IGenericRepository<Crash>>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.GetRepository<Crash>()).Returns(repositoryMock.Object);
        }

        [Fact]
        public async Task GetData_ReturnsCorrectData()
        {
            // Arrange
            var pageNumber = 1;
            var count = 2;

            unitOfWorkMock.Setup(u => u.GetRepository<Crash>().Get(It.IsAny<Expression<Func<Crash, bool>>>()))
                .Returns(data.AsQueryable().BuildMock());

            var documentService = new DocumentService(unitOfWorkMock.Object);

            // Act
            var result = await documentService.GetData<Crash>(pageNumber, count);

            // Assert
            Assert.Equal(count, result.Count);
            Assert.Equal(data[0].Year, result[0].Year);
            Assert.Equal(data[0].Month, result[0].Month);
            Assert.Equal(data[0].Day, result[0].Day);
            Assert.Equal(data[1].Year, result[1].Year);
            Assert.Equal(data[1].Month, result[1].Month);
            Assert.Equal(data[1].Day, result[1].Day);
        }

        [Fact]
        public void DownloadData_ReturnsFileContentResult()
        {
            // Arrange
            var data = new List<Crash>
            {
                new Crash { Id = Guid.NewGuid(), Year = 2021, Month = 6, Day = 1 },
                new Crash { Id = Guid.NewGuid(), Year = 2021, Month = 6, Day = 2 }
            };
            var entityType = typeof(Crash);

            var documentService = new DocumentService(unitOfWorkMock.Object);

            // Act
            var result = documentService.DownLoadData(data, entityType);

            // Assert
            Assert.IsType<FileContentResult>(result);

            Assert.Equal("text/csv", result.ContentType);
            Assert.Equal("Crash.csv", result.FileDownloadName);
        }

        [Fact]
        public async Task GetData_WithNoData_ReturnsEmptyList()
        {
            // Arrange
            var data = new List<Crash>();

            var pageNumber = 1;
            var count = 2;

            unitOfWorkMock.Setup(u => u.GetRepository<Crash>().Get(It.IsAny<Expression<Func<Crash, bool>>>()))
                .Returns(data.AsQueryable().BuildMock());

            var documentService = new DocumentService(unitOfWorkMock.Object);

            // Act
            var result = await documentService.GetData<Crash>(pageNumber, count);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void DownloadData_WithEmptyData_ReturnsFileContentResult()
        {
            // Arrange
            var data = new List<Crash>();

            var entityType = typeof(Crash);

            var documentService = new DocumentService(unitOfWorkMock.Object);

            // Act
            var result = documentService.DownLoadData(data, entityType);

            // Assert
            Assert.IsType<FileContentResult>(result);

            Assert.Equal("text/csv", result.ContentType);
            Assert.Equal("Crash.csv", result.FileDownloadName);
        }
    }
}
