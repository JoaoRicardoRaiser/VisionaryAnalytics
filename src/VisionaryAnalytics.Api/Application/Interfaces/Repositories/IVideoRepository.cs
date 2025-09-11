using MongoDB.Bson;
using VisionaryAnalytics.Api.Domain.Entities;

namespace VisionaryAnalytics.Api.Application.Interfaces.Repositories;

public interface IVideoRepository
{
    Task UpsertAsync(Video entity);
    Task<IEnumerable<Video>> GetAsync(ObjectId? id);
}
