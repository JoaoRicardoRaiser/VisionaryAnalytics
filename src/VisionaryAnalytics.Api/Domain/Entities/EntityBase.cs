using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace VisionaryAnalytics.Api.Domain.Entities
{
    public class EntityBase
    {
        [BsonId]
        [JsonIgnore]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
