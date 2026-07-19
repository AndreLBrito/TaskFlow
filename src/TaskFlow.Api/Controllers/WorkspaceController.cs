using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Features.Workspaces.GetWorkspaces;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/workspaces")]
public sealed class WorkspacesController : ControllerBase
{
    private readonly ISender _sender;

    public WorkspacesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
    CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetWorkspacesQuery(),
            cancellationToken);

        return Ok(result);
    }
}