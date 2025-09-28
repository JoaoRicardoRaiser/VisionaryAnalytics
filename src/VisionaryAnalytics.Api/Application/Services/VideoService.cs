using AutoMapper;
using Raisersoft.EasyRabbit.Interfaces;
using VisionaryAnalytics.Api.Application.Dtos;
using VisionaryAnalytics.Api.Application.Interfaces.Repositories;
using VisionaryAnalytics.Api.Application.Interfaces.Services;
using VisionaryAnalytics.Api.Domain.Entities;

namespace VisionaryAnalytics.Api.Application.Services;

public class VideoService(
    IConfiguration configuration,
    IMapper mapper, 
    IVideoRepository videoRepository,
    IMessagePublisherService<VideoReceivedEventDto> publisher) : IVideoService
{

    private readonly string PathToSaveVideo = configuration["PathToSaveVideos"]!;

    public async Task UploadAsync(IEnumerable<IFormFile> files)
    {
        foreach (var file in files)
        {
            var video = mapper.Map<Video>(file);
            await SaveVideo(file, video);

            await videoRepository.UpsertAsync(video);

            var eventDto = mapper.Map<VideoReceivedEventDto>(video);

            await publisher.SendMessageAsync(eventDto);
        }
    }

    private async Task SaveVideo(IFormFile file, Video video)
    {
        var directoryPath = Path.Combine(PathToSaveVideo, "VisionaryAnalytics");
        Directory.CreateDirectory(directoryPath);

        string filePath = Path.Combine(directoryPath, file.FileName);

        video.Path = filePath;

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
    }
}
