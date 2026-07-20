using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Features.Dashboard.GetDashboard;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public sealed class DashboardController : ControllerBase
{
    private readonly ISender _sender;

    public DashboardController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var dashboard = await _sender.Send(
            new GetDashboardQuery(),
            cancellationToken);

        return Ok(dashboard);
    }
}
