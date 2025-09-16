using MongoDB.Bson;
using VisionaryAnalytics.Worker.Domain.Entities;

namespace VisionaryAnalytics.Worker.Application.Interfaces.Repositories;

public interface IVideoRepository
{
    Task<Video?> GetAsync(ObjectId? id);
}
