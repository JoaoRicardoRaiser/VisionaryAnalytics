using VisionaryAnalytics.Api.Domain.Enums;

namespace VisionaryAnalytics.Api.Domain.Entities;

public class Video : EntityBase
{
    public string Name { get; set; } = default!;
    public long Lenght { get; set; }
    public string Path { get; set; } = default!;
    public VideoStatus Status { get; set; }
}
