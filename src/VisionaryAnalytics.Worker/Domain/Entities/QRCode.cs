namespace VisionaryAnalytics.Worker.Domain.Entities;

public class QRCode
{
    public int Frame { get; set; }
    public string Content { get; set; }
    public TimeSpan Time { get; set; }

    public QRCode(int frame, string content, TimeSpan time)
    {
        Frame = frame;
        Content = content;
        Time = time;
    }   
}
