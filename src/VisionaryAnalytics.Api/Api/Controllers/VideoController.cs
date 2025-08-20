using Microsoft.AspNetCore.Mvc;

namespace VisionaryAnalytics.Api.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class VideoController : Controller
{
    [HttpPost("/upload")]
    public IActionResult UploadAsync()
    {

        return Ok();
    }
}
