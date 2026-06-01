using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Features.Boards.CreateBoard;
using TaskFlow.Application.Features.Boards.DeleteBoard;
using TaskFlow.Application.Features.Boards.GetBoardById;
using TaskFlow.Application.Features.Boards.GetBoards;
using TaskFlow.Application.Features.Boards.UpdateBoard;
using TaskFlow.Web.Mapping;
using TaskFlow.Web.ViewModels.Boards.Create;
using TaskFlow.Web.ViewModels.Boards.Details;
using TaskFlow.Web.ViewModels.Boards.List;
using TaskFlow.Web.ViewModels.Boards.Update;

namespace TaskFlow.Web.Controllers;

public class BoardsController : Controller
{
    private readonly IMediator _mediator;

    public BoardsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(
        Guid workspaceId,
        CancellationToken cancellationToken)
    {
        var boards = await _mediator.Send(
            new GetBoardsQuery(workspaceId),
            cancellationToken);

        return View(new BoardListViewModel
        {
            WorkspaceId = workspaceId,
            Boards = boards
                .Select(board => board.To<BoardListItemViewModel>())
                .ToList()
        });
    }

    public IActionResult Create(Guid workspaceId)
    {
        return View(new CreateBoardViewModel
        {
            WorkspaceId = workspaceId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateBoardViewModel model,
        CancellationToken cancellationToken)
    {
        var boardId = await _mediator.Send(
            model.To<CreateBoardCommand>(),
            cancellationToken);

        return RedirectToAction(
            nameof(Details),
            new { id = boardId });
    }

    public async Task<IActionResult> Details(
        Guid id,
        CancellationToken cancellationToken)
    {
        var board = await _mediator.Send(
            new GetBoardByIdQuery(id),
            cancellationToken);

        if (board is null)
        {
            return NotFound();
        }

        return View(board.To<BoardDetailsViewModel>());
    }

    public async Task<IActionResult> Edit(
        Guid id,
        CancellationToken cancellationToken)
    {
        var board = await _mediator.Send(
            new GetBoardByIdQuery(id),
            cancellationToken);

        if (board is null)
        {
            return NotFound();
        }

        return View(board.To<UpdateBoardViewModel>());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        UpdateBoardViewModel model,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            model.To<UpdateBoardCommand>(),
            cancellationToken);

        return RedirectToAction(
            nameof(Details),
            new { id = model.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        Guid id,
        Guid workspaceId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteBoardCommand(id),
            cancellationToken);

        return RedirectToAction(
            nameof(Index),
            new { workspaceId });
    }
}