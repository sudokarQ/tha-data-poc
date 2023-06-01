namespace TheDataPOC.Tests
{
    using System.IO;
    using System.Threading.Tasks;

    using Application.Services;

    using Microsoft.AspNetCore.Http;

    public class UploadServiceTests
	{
        [Fact]
        public async void UploadCSVFilesNegative()
        {
            // Arrange
            UploadService uploadService = new UploadService();

            FormFile file;

            using (var stream = File.Create("placeholder.txt"))
            {
                file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                };
            }

            // Act
            Action act = () => uploadService.UploadFileAsync(file);

            //Assert
            Exception exception = Assert.Throws<Exception>(act);

            Assert.Equal("You can upload only .csv files", exception.Message);
        }

        [Fact]
        public async void UploadCSVFilesPositive()
        {
            // Arrange
            UploadService uploadService = new UploadService();

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

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task UploadServiceResultNotNullPositive()
        {
            // Arrange
            UploadService uploadService = new UploadService();

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

