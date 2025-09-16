using MongoDB.Bson;
using VisionaryAnalytics.Worker.Application.Interfaces.Repositories;
using VisionaryAnalytics.Worker.Application.Interfaces.Services;
using VisionaryAnalytics.Worker.Domain.Entities;

namespace VisionaryAnalytics.Worker.Application.Services;

public class VideoService(IVideoRepository videoRepository) : IVideoService
{
    public async Task ProcessVideo(ObjectId? videoId)
    {
        var video = await videoRepository.GetAsync(videoId);

        //TODO: make QR Code analysis.
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
