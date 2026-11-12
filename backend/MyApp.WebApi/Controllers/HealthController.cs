using Microsoft.AspNetCore.Mvc;

namespace MyApp.WebApi.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "OK" });
}
