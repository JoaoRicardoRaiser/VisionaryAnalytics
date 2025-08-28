using MongoDB.Driver;
using VisionaryAnalytics.Api.Domain.Entities;
using VisionaryAnalytics.Api.Domain.Interfaces;

namespace VisionaryAnalytics.Api.Infrastructure.Database.Repositories;

public class RepositoryBase<T>(MongoClient client) : IRepository<T> where T: EntityBase
{
    protected const string Database = "visionary_analytics_db";

    protected readonly IMongoCollection<T> Collection = client.GetDatabase(Database).GetCollection<T>(nameof(T));
    public async Task UpsertAsync(T entity)
    {
        var filter = Builders<T>.Filter.Eq(t => t.Id, entity.Id);
        await Collection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true });
    }
}
