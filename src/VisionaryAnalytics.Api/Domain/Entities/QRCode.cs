namespace VisionaryAnalytics.Api.Domain.Entities;

public class QRCode
{
    public int Frame { get; set; }
    public string Content { get; set; } = default!;
    public TimeSpan Time { get; set; }
}
