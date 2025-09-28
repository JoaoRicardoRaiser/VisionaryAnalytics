using MongoDB.Bson;
using VisionaryAnalytics.Worker.Domain.Entities;
using VisionaryAnalytics.Worker.Domain.Enums;

namespace VisionaryAnalytics.Worker.Application.Interfaces.Repositories;

public interface IVideoRepository
{
    Task<Video?> GetAsync(ObjectId? id);
    Task UpdateStatusAsync(ObjectId id, VideoStatus status);
    Task UpsertQrCodesAsync(ObjectId id, IEnumerable<QRCode> qrCodes);
}
