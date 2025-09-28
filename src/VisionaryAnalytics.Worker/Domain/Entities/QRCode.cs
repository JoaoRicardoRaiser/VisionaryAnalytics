namespace VisionaryAnalytics.Worker.Domain.Entities;

public class QRCode(int frame, string content, TimeSpan time)
{
    public int Frame { get; set; } = frame; 
    public string Content { get; set; } = content;
    public TimeSpan Time { get; set; } = time;
}
