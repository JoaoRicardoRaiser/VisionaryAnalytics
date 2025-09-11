using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using VisionaryAnalytics.Api.Application.Interfaces.Repositories;
using VisionaryAnalytics.Api.Application.Interfaces.Services;

namespace VisionaryAnalytics.Api.Presentation.Controllers;

[ApiController]
[Route("api/videos")]

public class VideoController(IVideoService videoService, IVideoRepository videoRepository) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadAsync(List<IFormFile> files)
    {
        await videoService.UploadAsync(files);
        return Created();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAsync([FromQuery] ObjectId? videoId)
        => Ok(await videoRepository.GetAsync(videoId));
}
