using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using MediatR;
using TaskFlow.Application.Features.Workspaces.CreateWorkspace;
using TaskFlow.Application.Features.Workspaces.GetWorkspaceById;
using TaskFlow.Application.Features.Workspaces.GetWorkspaces;

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
}