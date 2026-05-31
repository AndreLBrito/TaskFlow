using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using MediatR;
using TaskFlow.Application.Features.Workspaces.CreateWorkspace;
using TaskFlow.Application.Features.Workspaces.GetWorkspaceById;
using TaskFlow.Application.Features.Workspaces.GetWorkspaces;
using TaskFlow.Application.Features.Workspaces.UpdateWorkspace;
namespace TaskFlow.Web.Controllers;

public class WorkspacesController : Controller
{
    private readonly IMediator _mediator;

    public WorkspacesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var workspaces = await _mediator.Send(
            new GetWorkspacesQuery(),
            cancellationToken);

        return View(workspaces);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateWorkspaceCommand command,
        CancellationToken cancellationToken)
    {
        var workspaceId = await _mediator.Send(
            command,
            cancellationToken);

        return RedirectToAction(
            nameof(Details),
            new { id = workspaceId });
    }

    public async Task<IActionResult> Details(
        Guid id,
        CancellationToken cancellationToken)
    {
        var workspace = await _mediator.Send(
            new GetWorkspaceByIdQuery(id),
            cancellationToken);

        if (workspace is null)
        {
            return NotFound();
        }

        return View(workspace);
    }

    public async Task<IActionResult> Edit(
    Guid id,
    CancellationToken cancellationToken)
    {
        var workspace = await _mediator.Send(
            new GetWorkspaceByIdQuery(id),
            cancellationToken);

        if (workspace is null)
        {
            return NotFound();
        }

        var command = new UpdateWorkspaceCommand(
            workspace.Id,
            workspace.Name,
            workspace.Description);

        return View(command);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
    UpdateWorkspaceCommand command,
    CancellationToken cancellationToken)
    {
        await _mediator.Send(
            command,
            cancellationToken);

        return RedirectToAction(
            nameof(Details),
            new { id = command.Id });
    }
}