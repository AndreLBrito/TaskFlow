using Microsoft.AspNetCore.Mvc;
using MediatR;
using TaskFlow.Application.Features.Workspaces.CreateWorkspace;
using TaskFlow.Application.Features.Workspaces.GetWorkspaceById;
using TaskFlow.Application.Features.Workspaces.GetWorkspaces;
using TaskFlow.Application.Features.Workspaces.UpdateWorkspace;
namespace TaskFlow.Web.Controllers;

using TaskFlow.Web.ViewModels.Workspaces;

using TaskFlow.Application.Features.Workspaces.DeleteWorkspace;

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
        return View(new CreateWorkspaceViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateWorkspaceViewModel model,
        CancellationToken cancellationToken)
    {
        var command = new CreateWorkspaceCommand(
            model.Name,
            model.Description);

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

        var model = new UpdateWorkspaceViewModel
        {
            Id = workspace.Id,
            Name = workspace.Name,
            Description = workspace.Description
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        UpdateWorkspaceViewModel model,
        CancellationToken cancellationToken)
    {
        var command = new UpdateWorkspaceCommand(
            model.Id,
            model.Name,
            model.Description);

        await _mediator.Send(
            command,
            cancellationToken);

        return RedirectToAction(
            nameof(Details),
            new { id = model.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
    Guid id,
    CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteWorkspaceCommand(id),
            cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}