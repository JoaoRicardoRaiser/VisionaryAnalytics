using MongoDB.Bson;
using MongoDB.Driver;
using VisionaryAnalytics.Api.Application.Interfaces.Repositories;
using VisionaryAnalytics.Api.Domain.Entities;

namespace VisionaryAnalytics.Api.Infrastructure.Database.Repositories;

public class VideoRepository(MongoClient client) : IVideoRepository
{
    protected const string Database = "visionary_analytics_db";

    protected readonly IMongoCollection<Video> Collection = client.GetDatabase(Database).GetCollection<Video>(nameof(Video));

    public async Task<IEnumerable<Video>> GetAsync(ObjectId? id)
    {
        var filter = Builders<Video>.Filter.Eq(t => t.Id, id);

        var result = id == null
            ? await Collection.FindAsync(x => true)
            : await Collection.FindAsync(filter);

        return await result.ToListAsync();
    }

    public async Task UpsertAsync(Video entity)
    {
        var filter = Builders<Video>.Filter.Eq(t => t.Id, entity.Id);
        await Collection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true });
    }
}
