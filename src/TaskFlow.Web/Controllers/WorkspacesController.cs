using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Features.Workspaces.CreateWorkspace;
using TaskFlow.Application.Features.Workspaces.GetWorkspaceById;

namespace TaskFlow.Web.Controllers;

public class WorkspacesController : Controller
{
    private readonly IMediator _mediator;

    public WorkspacesController(IMediator mediator)
    {
        _mediator = mediator;
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
        var workspaceId = await _mediator.Send(command, cancellationToken);

        return RedirectToAction(nameof(Details), new { id = workspaceId });
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
}