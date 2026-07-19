using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Ok(new
        {
            status = "healthy",
            application = "TaskFlow.Api",
            timestamp = DateTimeOffset.UtcNow
        });
    }
}
