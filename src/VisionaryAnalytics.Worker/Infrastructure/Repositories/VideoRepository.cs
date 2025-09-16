using MongoDB.Bson;
using MongoDB.Driver;
using VisionaryAnalytics.Worker.Application.Interfaces.Repositories;
using VisionaryAnalytics.Worker.Domain.Entities;

namespace VisionaryAnalytics.Worker.Infrastructure.Database.Repositories;

public class VideoRepository(MongoClient client) : IVideoRepository
{
    protected const string Database = "visionary_analytics_db";

    protected readonly IMongoCollection<Video> Collection = client.GetDatabase(Database).GetCollection<Video>(nameof(Video));

    public async Task<Video?> GetAsync(ObjectId? id)
    {
        var filter = Builders<Video>.Filter.Eq(t => t.Id, id);
        var result = await Collection.FindAsync(filter);

        return await result.SingleOrDefaultAsync();
    }
}
