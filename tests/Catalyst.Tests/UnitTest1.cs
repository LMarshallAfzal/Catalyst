using Xunit;
using Moq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

using Catalyst.Controllers;
using Catalyst.Models;

namespace Catalyst.Tests
{
    public class FileControllerTests
    {
        [Fact]
        public async void Upload_ValidFile_ReturnSuccess()
        {
            // Arrange
            var MockFileStorage = new Mock<IFileStorage>();
            MockFileStorage.Setup(x => x.SaveFileAsync(It.IsAny<string>(), It.IsAny<Stream>())).Returns(Task.CompletedTask);

            var controller = new FilesController(MockFileStorage.Object);
            var testFile = CreateTestFile();
            var mockFormFile = CreateMockFormFile("someFileName.txt", testFile);

            controller.Request.Form = new FormCollection(new Dictionary<string, StringValues>
            {
                { "files", mockFormFile.FileName }
            });

            // Act
            var result = await controller.Upload();

            // Assert
            Assert.IsType<CreatedResult>(result);
            var createdResult = result as CreatedResult;
            Assert.Equal("someFileName.txt", createdResult.Value);
        }

        [Fact]
        public void Upload_InvalidFile_ReturnFailure()
        {

        }

        [Fact]
        public void Upload_Storage_Failure()
        {

        }

        private static MemoryStream CreateTestFile()
        {
            var content = "This is some samle content for the test file.";
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);
            return stream;
        }

        private static IFormFile CreateMockFormFile(string fileName, Stream contentStream)
        {
            var mockFormFile = new Mock<IFormFile>();
            mockFormFile.Setup(_ => _.FileName).Returns(fileName);
            mockFormFile.Setup(_ => _.OpenReadStream()).Returns(contentStream);

            return mockFormFile.Object;
        }
    }
}