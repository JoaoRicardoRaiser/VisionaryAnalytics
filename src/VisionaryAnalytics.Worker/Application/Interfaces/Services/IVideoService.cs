using MongoDB.Bson;

namespace VisionaryAnalytics.Worker.Application.Interfaces.Services;

public interface IVideoService
{
    Task ProcessVideo(ObjectId? videoId);
}
