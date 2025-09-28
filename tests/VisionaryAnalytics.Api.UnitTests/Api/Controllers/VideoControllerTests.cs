using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using System.Net;
using VisionaryAnalytics.Api.Application.Interfaces.Repositories;
using VisionaryAnalytics.Api.Application.Interfaces.Services;
using VisionaryAnalytics.Api.Domain.Entities;
using VisionaryAnalytics.Api.Domain.Enums;
using VisionaryAnalytics.Api.Presentation.Controllers;

namespace VisionaryAnalytics.Api.UnitTests.Api.Controllers;

public class VideoControllerTests
{

    private readonly Mock<IVideoService> _videoServiceMock = new();
    private readonly Mock<IVideoRepository> _videoRepositoryMock = new();
    private readonly VideoController _videoController;

    public VideoControllerTests()
        => _videoController = new(_videoServiceMock.Object, _videoRepositoryMock.Object);

    [Fact]
    public async Task Should_Return_BadRequest_When_No_Files_Are_Sent()
    {
        // Arrange
        List<IFormFile> files = [];

        // Act
        var result = await _videoController.UploadAsync(files);

        // Assert
        var badRequestObjectResult = result as BadRequestObjectResult;

        Assert.Equal((int)HttpStatusCode.BadRequest, badRequestObjectResult!.StatusCode);
        Assert.Equal("No files sent.", badRequestObjectResult!.Value);
    }

    [Fact]
    public async Task Should_Return_BadRequest_When_Extensions_File_Is_Invalid()
    {
        // Arrange
        var fileContent = "This is a test file for upload.";
        var fileName = "mytestfile.txt";
        var contentType = "text/plain";

        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        writer.Write(fileContent);
        writer.Flush();
        stream.Position = 0;

        IFormFile testFile = new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };

        // Act
        var result = await _videoController.UploadAsync([testFile]);

        // Assert
        var badRequestObjectResult = result as BadRequestObjectResult;

        Assert.Equal((int)HttpStatusCode.BadRequest, badRequestObjectResult!.StatusCode);
        Assert.Equal($"Only files with .mp4, .avi extensions are accepted.\n" +
                     $"Invalid files: mytestfile.txt", badRequestObjectResult!.Value);
    }

    [Theory]
    [InlineData("mp4")]
    [InlineData("MP4")]
    [InlineData("avi")]
    [InlineData("AVI")]
    public async Task Should_Return_Created_When_Extensions_File_Is_Valid(string extension)
    {
        // Arrange
        var fileContent = "This is a test file for upload.";
        var fileName = $"mytestfile.{extension}";
        var contentType = "text/plain";

        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        writer.Write(fileContent);
        writer.Flush();
        stream.Position = 0;

        IFormFile testFile = new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };

        List<IFormFile> filesToUpload = [testFile];

        // Act
        var result = await _videoController.UploadAsync(filesToUpload);

        // Assert
        _videoServiceMock.Verify(s => s.UploadAsync(filesToUpload), Times.Once);
        Assert.IsType<CreatedResult>(result);
    }

    [Fact]
    public async Task Should_Get_All_Saved_Videos()
    {
        // Arrange
        List<Video> savedVideos =
        [
            new Video
            {
                Id = ObjectId.GenerateNewId(),
                LengthInMb = 100,
                Name = "testFile.MP4",
                Path = "test/video/testFile.MP4",
                Status = VideoStatus.Enqueued,
                QRCodes = [],
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new Video
            {
                Id = ObjectId.GenerateNewId(),
                LengthInMb = 100,
                Name = "video1.avi",
                Path = "test/video/video1.avi",
                Status = VideoStatus.Finished,
                QRCodes =
                [
                    new QRCode
                    {
                        Content = "abcdef",
                        Frame = 323,
                        Time = new TimeSpan(0, 3, 27)
                    },
                    new QRCode
                    {
                        Content = "xyz",
                        Frame = 932,
                        Time = new TimeSpan(0, 8, 3)
                    }
                ],
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            }
        ];

        _videoRepositoryMock
            .Setup(x => x.GetAsync(null))
            .ReturnsAsync(savedVideos);

        // Act
        var result = await _videoController.GetAsync(null);
        var okObjectResult = result as OkObjectResult;

        // Assert

        _videoRepositoryMock.Verify(x => x.GetAsync(null), Times.Once);
        Assert.Equal(savedVideos, okObjectResult!.Value);
    }

    [Fact]
    public async Task Should_Get_Video_By_Id()
    {
        // Arrange
        var queryVideoId = ObjectId.GenerateNewId();

        List<Video> savedVideos =
        [
            new Video
            {
                Id = ObjectId.GenerateNewId(),
                LengthInMb = 100,
                Name = "testFile.MP4",
                Path = "test/video/testFile.MP4",
                Status = VideoStatus.Enqueued,
                QRCodes = [],
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new Video
            {
                Id = queryVideoId,
                LengthInMb = 100,
                Name = "video1.avi",
                Path = "test/video/video1.avi",
                Status = VideoStatus.Finished,
                QRCodes =
                [
                    new QRCode
                    {
                        Content = "abcdef",
                        Frame = 323,
                        Time = new TimeSpan(0, 3, 27)
                    },
                    new QRCode
                    {
                        Content = "xyz",
                        Frame = 932,
                        Time = new TimeSpan(0, 8, 3)
                    }
                ],
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            }
        ];

        _videoRepositoryMock
            .Setup(x => x.GetAsync(queryVideoId))
            .ReturnsAsync([savedVideos.ElementAt(1)]);

        // Act
        var result = await _videoController.GetAsync(queryVideoId);
        var okObjectResult = result as OkObjectResult;

        // Assert
        List<Video> expectedVideo = [savedVideos.ElementAt(1)];

        _videoRepositoryMock.Verify(x => x.GetAsync(queryVideoId), Times.Once);
        Assert.Equal(expectedVideo, okObjectResult!.Value);
    }
}
