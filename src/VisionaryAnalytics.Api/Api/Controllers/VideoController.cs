using Microsoft.AspNetCore.Mvc;
using VisionaryAnalytics.Api.Application.Interfaces;

namespace VisionaryAnalytics.Api.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class VideoController : Controller
{
    [HttpPost("/upload")]
    public async Task<IActionResult> UploadAsync(
        List<IFormFile> files,
        [FromServices] IVideoService videoSerivce)
    {

        await videoSerivce.UploadAsync(files);
        
        return Ok();
    }
}
