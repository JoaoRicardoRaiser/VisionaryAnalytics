using MongoDB.Bson;

namespace VisionaryAnalytics.Api.Domain.Entities
{
    public class EntityBase
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
