using MongoDB.Bson;

namespace VisionaryAnalytics.Api.Application.Dtos;

public record VideoReceivedEventDto
{
    public ObjectId Id { get; set; }
}
