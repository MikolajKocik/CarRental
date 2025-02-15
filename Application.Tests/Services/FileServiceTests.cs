using Microsoft.AspNetCore.Hosting;
using Moq;
using Microsoft.AspNetCore.Http;

namespace CarRental.Application.Services.Tests
{
    public class FileServiceTests
    {
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly FileService _fileService;
        private readonly string _testFolder = "uploads"; // test folder

        public FileServiceTests()
        {
            // Create mock IWebHostEnvironment
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            
            // Set WebRootPath to return path to the temporary Catalog
            _mockWebHostEnvironment.Setup(env => env.WebRootPath).Returns(Path.GetTempPath());

            // Create object FileService with mocked IWebHostEnvironment
            _fileService = new FileService(_mockWebHostEnvironment.Object);
        }

        [Fact()]
        public async Task SaveFilesAsync_ShouldReturnEmptyList_WhenNoFilesProvided()
        {
            // Arrange
            var files = new List<IFormFile>(); // empty objects list

            // Act
            var result = await _fileService.SaveFilesAsync(files, _testFolder);

            // Assert
            Assert.Empty(result);
        }

        [Fact()]
        public async Task SaveFilesAsync_ShoulSaveFiles_WhenValidFilesProvided()
        {
            // Arrange
            var files = new List<IFormFile>
            {
                CreateMockFile("test1.txt", "Test file content"),
                CreateMockFile("test2.jpg", "Another test file")
            };

            // Act
            var result = await _fileService.SaveFilesAsync(files, _testFolder);

            // Assert
            Assert.Equal(2, result.Count); // Checks if 2 files saved

            foreach(var filePath in result)
            {
                Assert.Contains(_testFolder, filePath); // Checks whether path is valid

                var fullPath = Path.Combine(_mockWebHostEnvironment.Object.WebRootPath, filePath);
                Assert.True(File.Exists(fullPath)); // Checks if file exists
            }
        }

        /// <summary>
        /// Creates a temporary mock file to tests
        /// </summary>
        /// <param name="fileName">Object's file name</param>
        /// <param name="content">Object's content</param>
        /// <returns>Temporary file as mock object</returns>

        private IFormFile CreateMockFile(string fileName, string content)
        {
            var mockFile = new Mock<IFormFile>();
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            mockFile.Setup(f => f.FileName).Returns(fileName);
            mockFile.Setup(f => f.Length).Returns(stream.Length);
            mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
            mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream targetStream, CancellationToken _) =>
                {
                    stream.CopyTo(targetStream);
                    return Task.CompletedTask;
                });

            return mockFile.Object;
        }

        [Fact()]
        public async Task DeleteFilesAsync_ShouldDeleteFiles_WhenTheyExist()
        {
            // Arrange
            var testFile = "test.txt";
            var testFolder = Path.Combine(_mockWebHostEnvironment.Object.WebRootPath, _testFolder);

            if(!Directory.Exists(testFolder))
            {
                Directory.CreateDirectory(testFolder);
            }

            var testFilePath = Path.Combine(testFolder, testFile);
            await File.WriteAllTextAsync(testFilePath, "Test");

            var paths = new List<string> { Path.Combine(_mockWebHostEnvironment.Object.WebRootPath, testFile) };

            // Act

            await _fileService.DeleteFilesAsync(paths, _testFolder);

            // Assert

            Assert.True(File.Exists(testFilePath));
        }

        [Fact()]
        public async Task DeleteFileAsync_ShouldNotThrow_WhenFileDoesNotExist()
        {
            // Arange
            var paths = new List<string> { "images/notexistfile.txt" };

            // Act

            var exception = await Record.ExceptionAsync(() => _fileService.DeleteFilesAsync(paths, _testFolder));

            // Assert

            Assert.Null(exception);
        }

        [Fact()]
        public async Task DeleteFileAsync_ShouldNotThrow_WhenNoFilesProvided()  
        {
            // Arange
            var paths = new List<string> { "images/notexistfile.txt" };

            // Act

            var exception = await Record.ExceptionAsync(() => _fileService.DeleteFilesAsync(paths, _testFolder));

            // Assert

            Assert.Null(exception);
        }
    }
}