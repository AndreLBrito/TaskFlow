using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Features.TaskItems.CreateTaskItem;
using TaskFlow.Application.Features.TaskItems.GetTaskItemById;
using TaskFlow.Application.Features.TaskItems.UpdateTaskItem;
using TaskFlow.Web.Mapping;
using TaskFlow.Web.ViewModels.TaskItems.Create;
using TaskFlow.Web.ViewModels.TaskItems.Details;
using TaskFlow.Web.ViewModels.TaskItems.Update;

namespace TaskFlow.Web.Controllers;

public class TaskItemsController : Controller
{
    private readonly IMediator _mediator;

    public TaskItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IActionResult Create(
        Guid boardId,
        Guid boardColumnId)
    {
        return View(new CreateTaskItemViewModel
        {
            BoardId = boardId,
            BoardColumnId = boardColumnId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateTaskItemViewModel model,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            model.To<CreateTaskItemCommand>(),
            cancellationToken);

        return RedirectToAction(
            "Details",
            "Boards",
            new { id = model.BoardId });
    }

    public async Task<IActionResult> Details(
        Guid id,
        CancellationToken cancellationToken)
    {
        var task = await _mediator.Send(
            new GetTaskItemByIdQuery(id),
            cancellationToken);

        if (task is null)
        {
            return NotFound();
        }

        return View(
            task.To<TaskItemDetailsViewModel>());
    }

    public async Task<IActionResult> Edit(
        Guid id,
        CancellationToken cancellationToken)
    {
        var task = await _mediator.Send(
            new GetTaskItemByIdQuery(id),
            cancellationToken);

        if (task is null)
        {
            return NotFound();
        }

        return View(
            task.To<UpdateTaskItemViewModel>());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        UpdateTaskItemViewModel model,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            model.To<UpdateTaskItemCommand>(),
            cancellationToken);

        return RedirectToAction(
            nameof(Details),
            new { id = model.Id });
    }
}