using MongoDB.Bson;
using MongoDB.Driver;
using VisionaryAnalytics.Worker.Application.Interfaces.Repositories;
using VisionaryAnalytics.Worker.Domain.Entities;
using VisionaryAnalytics.Worker.Domain.Enums;

namespace VisionaryAnalytics.Worker.Infrastructure.Database.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly MongoClient _client;
    private const string Database = "visionary_analytics_db";
    private readonly IMongoCollection<Video> _collection;

    public VideoRepository(MongoClient client)
    {
        _client = client;
        _collection = client.GetDatabase(Database).GetCollection<Video>(nameof(Video));
    }

    public async Task<Video?> GetAsync(ObjectId? id)
    {
        var filter = Builders<Video>.Filter.Eq(t => t.Id, id);
        var result = await _collection.FindAsync(filter);

        return await result.SingleOrDefaultAsync();
    }

    public async Task UpdateStatusAsync(ObjectId id, VideoStatus status)
    {
        var filter = Builders<Video>.Filter.Eq(t => t.Id, id);
        var update = Builders<Video>.Update.Set(u => u.Status, status);

        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task UpsertQrCodesAsync(ObjectId id, IEnumerable<QRCode> qrCodes)
    {
        var filter = Builders<Video>.Filter.Eq(t => t.Id, id);
        var update = Builders<Video>.Update.Set(u => u.QRCodes, qrCodes);

        await _collection.UpdateOneAsync(filter, update);
    }
}
