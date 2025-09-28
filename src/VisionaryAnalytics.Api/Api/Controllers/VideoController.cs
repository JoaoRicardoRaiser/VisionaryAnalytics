using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using VisionaryAnalytics.Api.Application.Interfaces.Repositories;
using VisionaryAnalytics.Api.Application.Interfaces.Services;

namespace VisionaryAnalytics.Api.Presentation.Controllers;

[ApiController]
[Route("api/videos")]

public class VideoController(IVideoService videoService, IVideoRepository videoRepository) : ControllerBase
{
    private static readonly string[] _validExtensions = [".mp4", ".avi"];

    [HttpPost("upload")]
    public async Task<IActionResult> UploadAsync(List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
            return BadRequest("No files sent.");

        var invalidFiles = files
            .Where(f => !_validExtensions.Contains(Path.GetExtension(f.FileName).ToLower()))
            .ToList();

        if (invalidFiles.Count != 0)
        {
            return BadRequest($"Only files with {string.Join(", ", _validExtensions)} extensions are accepted.\n" +
                              $"Invalid files: {string.Join(", ", invalidFiles.Select(f => f.FileName))}");
        }

        await videoService.UploadAsync(files);
        return Created();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAsync([FromQuery] ObjectId? id)
        => Ok(await videoRepository.GetAsync(id));
}
