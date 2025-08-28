using AutoMapper;
using VisionaryAnalytics.Api.Application.Interfaces;
using VisionaryAnalytics.Api.Domain.Entities;
using VisionaryAnalytics.Api.Domain.Interfaces;

namespace VisionaryAnalytics.Api.Application.Services;

public class VideoService(IMapper mapper, IRepository<Video> videoRepository) : IVideoService
{
    public async Task UploadAsync(IEnumerable<IFormFile> files)
    {

        foreach (var file in files)
        {
            var video = mapper.Map<Video>(file);
            await SaveVideo(file, video);

            await videoRepository.UpsertAsync(video);

            //var eventDto = mapper.Map<VideoReceivedEventDto>(video);
        }
    }

    private static async Task SaveVideo(IFormFile file, Video video)
    {
        var directoryPath = Path.Combine(Path.GetTempPath(), "VisionaryAnalytics");
        Directory.CreateDirectory(directoryPath);

        string filePath = Path.Combine(directoryPath, file.FileName);

        video.Path = filePath;

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
    }
}
