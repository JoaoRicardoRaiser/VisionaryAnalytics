using VisionaryAnalytics.Api.Domain.Enums;

namespace VisionaryAnalytics.Api.Domain.Entities;

public class Video : EntityBase
{
    public string Link { get; set; } = default!;
    public VideoStatus Status { get; set; }
}
