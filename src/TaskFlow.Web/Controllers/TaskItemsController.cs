using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Features.TaskItems.CreateTaskItem;
using TaskFlow.Web.Mapping;
using TaskFlow.Web.ViewModels.TaskItems.Create;

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
}