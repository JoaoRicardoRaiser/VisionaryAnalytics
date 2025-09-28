using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Raisersoft.EasyRabbit.Interfaces;
using System.Text;
using VisionaryAnalytics.Api.Application.Dtos;
using VisionaryAnalytics.Api.Application.Interfaces.Repositories;
using VisionaryAnalytics.Api.Application.Services;
using VisionaryAnalytics.Api.Domain.Entities;

namespace VisionaryAnalytics.Api.UnitTests.Application.Services;
public class VideoServiceTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IVideoRepository> _videoRepositoryMock = new();
    private readonly Mock<IMessagePublisherService<VideoReceivedEventDto>> _publisherMock = new();
    private readonly VideoService _videoService;

    public VideoServiceTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new KeyValuePair<string, string?>("PathToSaveVideos", "Local")
            ]);

        _videoService = new VideoService(
            configuration.Build(),
            _mapperMock.Object,
            _videoRepositoryMock.Object,
            _publisherMock.Object);
    }

    [Fact]
    public async Task Should_Map_And_Save_And_Publish_ForEachFile()
    {
        // Arrange
        var files = new List<IFormFile>
        {
            CreateFakeFormFile("video1.mp4"),
            CreateFakeFormFile("video2.mp4")
        };

        var videoMapped = new Video();
        var eventDtoMapped = new VideoReceivedEventDto();

        _mapperMock.Setup(m => m.Map<Video>(It.IsAny<IFormFile>()))
            .Returns(videoMapped);

        _mapperMock.Setup(m => m.Map<VideoReceivedEventDto>(It.IsAny<Video>()))
            .Returns(eventDtoMapped);

        // Act
        await _videoService.UploadAsync(files);

        // Assert
        _mapperMock.Verify(m => m.Map<Video>(It.IsAny<IFormFile>()), Times.Exactly(files.Count));
        _videoRepositoryMock.Verify(r => r.UpsertAsync(It.IsAny<Video>()), Times.Exactly(files.Count));
        _mapperMock.Verify(m => m.Map<VideoReceivedEventDto>(It.IsAny<Video>()), Times.Exactly(files.Count));
        _publisherMock.Verify(p => p.SendMessageAsync(It.IsAny<VideoReceivedEventDto>()), Times.Exactly(files.Count));
    }

    [Fact]
    public async Task Should_Save_File_To_TempPath()
    {
        // Arrange
        var fileName = "video-test.mp4";
        var file = CreateFakeFormFile(fileName, "some data");

        var videoMapped = new Video();
        _mapperMock.Setup(m => m.Map<Video>(It.IsAny<IFormFile>()))
            .Returns(videoMapped);

        _mapperMock.Setup(m => m.Map<VideoReceivedEventDto>(It.IsAny<Video>()))
            .Returns(new VideoReceivedEventDto());

        var files = new List<IFormFile> { file };

        // Act
        await _videoService.UploadAsync(files);

        // Assert
        var expectedPath = Path.Combine("Local", "VisionaryAnalytics", fileName);

        Assert.Equal(expectedPath, videoMapped.Path);
        Assert.True(File.Exists(expectedPath));

        File.Delete(expectedPath);
    }

    private static FormFile CreateFakeFormFile(string fileName, string content = "dummy content")
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        return new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "video/mp4"
        };
    }
}
