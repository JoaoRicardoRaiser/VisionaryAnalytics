using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using VisionaryAnalytics.Worker.Application.Interfaces.Repositories;
using VisionaryAnalytics.Worker.Application.Services;
using VisionaryAnalytics.Worker.Domain.Entities;
using VisionaryAnalytics.Worker.Domain.Enums;
using VisionaryAnalytics.Worker.UnitTests.Helpers;

namespace VisionaryAnalytics.Worker.UnitTests.Application;

public class VideoServiceTests
{
    private readonly Mock<ILogger<VideoService>> _loggerMock;
    private readonly Mock<IVideoRepository> _repoMock;
    private readonly VideoService _service;

    public VideoServiceTests()
    {
        _loggerMock = new Mock<ILogger<VideoService>>();
        _repoMock = new Mock<IVideoRepository>();
        _service = new VideoService(_loggerMock.Object, _repoMock.Object);
    }

    [Fact]
    public async Task ProcessVideo_Should_Log_And_Return_When_Video_NotFound()
    {
        // Arrange
        ObjectId id = ObjectId.GenerateNewId();
        _repoMock.Setup(r => r.GetAsync(id)).ReturnsAsync((Video?)null);

        // Act
        await _service.ProcessVideo(id);

        // Assert
        _repoMock.Verify(r => r.UpdateStatusAsync(It.IsAny<ObjectId>(), It.IsAny<VideoStatus>()), Times.Never);
        _repoMock.Verify(r => r.UpsertQrCodesAsync(It.IsAny<ObjectId>(), It.IsAny<IEnumerable<QRCode>>()), Times.Never);

        _loggerMock.VerifyLog(LogLevel.Information, $"Video not found. id: {id}", Times.Once());
    }

    [Fact]
    public async Task SaveQrCodeInfosAsync_Should_Log_When_No_QRCode_Found()
    {
        // Arrange
        var video = new Video
        {
            Id = ObjectId.GenerateNewId(),
            Path = "fake.mp4"
        };

        var method = typeof(VideoService).GetMethod("SaveQrCodeInfosAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        await (Task)method.Invoke(_service, new object[] { new List<QRCode>(), video });

        // Assert
        _repoMock.Verify(r => r.UpsertQrCodesAsync(It.IsAny<ObjectId>(), It.IsAny<IEnumerable<QRCode>>()), Times.Never);
        _loggerMock.VerifyLog(LogLevel.Information, $"QR codes not found for video: {video.Path}", Times.Once());
    }

    [Fact]
    public async Task SaveQrCodeInfosAsync_Should_Call_Repository_When_QRCodes_Found()
    {
        // Arrange
        var video = new Video
        {
            Id = ObjectId.GenerateNewId(),
            Path = "fake.mp4"
        };

        var qrCodes = new List<QRCode>
        {
            new QRCode(1, "content1", TimeSpan.FromSeconds(1)),
            new QRCode(2, "content2", TimeSpan.FromSeconds(2))
        };

        var method = typeof(VideoService).GetMethod("SaveQrCodeInfosAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        await (Task)method.Invoke(_service, new object[] { qrCodes, video });

        // Assert
        _repoMock.Verify(r => r.UpsertQrCodesAsync(video.Id, qrCodes), Times.Once);
    }
}
