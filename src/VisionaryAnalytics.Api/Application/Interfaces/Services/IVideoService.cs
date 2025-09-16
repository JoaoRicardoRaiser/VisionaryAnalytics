namespace VisionaryAnalytics.Api.Application.Interfaces.Services;

public interface IVideoService
{
    Task UploadAsync(IEnumerable<IFormFile> files);
}
