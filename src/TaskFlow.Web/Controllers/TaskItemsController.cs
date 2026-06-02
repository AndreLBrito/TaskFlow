using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskFlow.Application.Features.BoardColumns.GetBoardColumns;
using TaskFlow.Application.Features.TaskItems.CreateTaskItem;
using TaskFlow.Application.Features.TaskItems.DeleteTaskItem;
using TaskFlow.Application.Features.TaskItems.GetTaskItemById;
using TaskFlow.Application.Features.TaskItems.MoveTaskItem;
using TaskFlow.Application.Features.TaskItems.UpdateTaskItem;
using TaskFlow.Web.Mapping;
using TaskFlow.Web.ViewModels.TaskItems.Create;
using TaskFlow.Web.ViewModels.TaskItems.Details;
using TaskFlow.Web.ViewModels.TaskItems.Move;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        Guid id,
        Guid boardId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteTaskItemCommand(id),
            cancellationToken);

        return RedirectToAction(
            "Details",
            "Boards",
            new { id = boardId });
    }

    public async Task<IActionResult> Move(
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

        var columns = await _mediator.Send(
            new GetBoardColumnsQuery(task.BoardId),
            cancellationToken);

        var model = new MoveTaskItemViewModel
        {
            Id = task.Id,
            BoardId = task.BoardId,
            BoardColumnId = task.BoardColumnId,
            Order = task.Order,
            Columns = columns
                .Select(column => new SelectListItem
                {
                    Value = column.Id.ToString(),
                    Text = column.Name
                })
                .ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Move(
        MoveTaskItemViewModel model,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            model.To<MoveTaskItemCommand>(),
            cancellationToken);

        return RedirectToAction(
            "Details",
            "Boards",
            new { id = model.BoardId });
    }
}