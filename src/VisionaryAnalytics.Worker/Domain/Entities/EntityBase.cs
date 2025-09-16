using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VisionaryAnalytics.Worker.Domain.Entities
{
    public class EntityBase
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
