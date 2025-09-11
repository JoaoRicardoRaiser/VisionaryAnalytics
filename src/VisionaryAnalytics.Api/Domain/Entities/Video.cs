using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VisionaryAnalytics.Api.Domain.Enums;

namespace VisionaryAnalytics.Api.Domain.Entities;

public class Video : EntityBase
{
    public string Name { get; set; } = default!;
    public long Length { get; set; }
    public string Path { get; set; } = default!;

    [BsonRepresentation(BsonType.String)]
    public VideoStatus Status { get; set; }
}
