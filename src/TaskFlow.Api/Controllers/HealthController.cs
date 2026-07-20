using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts.Common;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public IActionResult Get(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Ok(new HealthResponse(
            "healthy",
            "TaskFlow.Api",
            DateTimeOffset.UtcNow));
    }
}
