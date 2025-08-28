namespace VisionaryAnalytics.Api.Application.Interfaces;

public interface IVideoService
{
    Task UploadAsync(IEnumerable<IFormFile> files);
}
