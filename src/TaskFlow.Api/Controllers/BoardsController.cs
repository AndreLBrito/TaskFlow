using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts.Boards;
using TaskFlow.Api.Contracts.Common;
using TaskFlow.Application.Features.Boards.CreateBoard;
using TaskFlow.Application.Features.Boards.DeleteBoard;
using TaskFlow.Application.Features.Boards.GetBoardById;
using TaskFlow.Application.Features.Boards.GetBoardKanban;
using TaskFlow.Application.Features.Boards.GetBoards;
using TaskFlow.Application.Features.Boards.UpdateBoard;
using TaskFlow.Application.Features.Workspaces.GetWorkspaceById;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class BoardsController : ControllerBase
{
    private readonly ISender _sender;

    public BoardsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("workspaces/{workspaceId:guid}/boards")]
    [ProducesResponseType(typeof(IReadOnlyList<BoardListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByWorkspace(
        Guid workspaceId,
        CancellationToken cancellationToken)
    {
        var workspace = await _sender.Send(
            new GetWorkspaceByIdQuery(workspaceId),
            cancellationToken);

        if (workspace is null)
        {
            return NotFound(
                ApiProblemDetails.NotFound("Workspace não encontrado."));
        }

        var boards = await _sender.Send(
            new GetBoardsQuery(workspaceId),
            cancellationToken);

        return Ok(boards);
    }

    [HttpGet("boards/{id:guid}")]
    [ProducesResponseType(typeof(BoardDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var board = await _sender.Send(
            new GetBoardByIdQuery(id),
            cancellationToken);

        return board is null
            ? NotFound(ApiProblemDetails.NotFound("Quadro não encontrado."))
            : Ok(board);
    }

    [HttpPost("workspaces/{workspaceId:guid}/boards")]
    [ProducesResponseType(typeof(IdResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(
        Guid workspaceId,
        [FromBody] CreateBoardRequest request,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            new CreateBoardCommand(
                workspaceId,
                request.Name,
                request.Description),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new IdResponse(id));
    }

    [HttpPut("boards/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateBoardRequest request,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new UpdateBoardCommand(
                id,
                request.Name,
                request.Description),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("boards/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteBoardCommand(id),
            cancellationToken);

        return NoContent();
    }

    [HttpGet("boards/{id:guid}/kanban")]
    [ProducesResponseType(typeof(KanbanBoardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetKanban(
        Guid id,
        CancellationToken cancellationToken)
    {
        var board = await _sender.Send(
            new GetBoardKanbanQuery(id),
            cancellationToken);

        return board is null
            ? NotFound(ApiProblemDetails.NotFound("Quadro não encontrado."))
            : Ok(board);
    }
}
