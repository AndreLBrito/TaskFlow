using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Features.Workspaces.CreateWorkspace;
using TaskFlow.Application.Features.Workspaces.DeleteWorkspace;
using TaskFlow.Application.Features.Workspaces.GetWorkspaceById;
using TaskFlow.Application.Features.Workspaces.GetWorkspaces;
using TaskFlow.Application.Features.Workspaces.UpdateWorkspace;

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
    [ProducesResponseType(typeof(IEnumerable<WorkspaceListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetWorkspacesQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WorkspaceDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetWorkspaceByIdQuery(id),
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new { id });
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateWorkspaceCommand(
            id,
            request.Name,
            request.Description);

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteWorkspaceCommand(id),
            cancellationToken);

        return NoContent();
    }
}

public sealed record UpdateWorkspaceRequest(
    string? Name,
    string? Description);
