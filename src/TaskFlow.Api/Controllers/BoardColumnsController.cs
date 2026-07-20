using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts.BoardColumns;
using TaskFlow.Api.Contracts.Common;
using TaskFlow.Application.Features.BoardColumns.CreateBoardColumn;
using TaskFlow.Application.Features.BoardColumns.DeleteBoardColumn;
using TaskFlow.Application.Features.BoardColumns.GetBoardColumnById;
using TaskFlow.Application.Features.BoardColumns.GetBoardColumns;
using TaskFlow.Application.Features.BoardColumns.ReorderBoardColumns;
using TaskFlow.Application.Features.BoardColumns.UpdateBoardColumn;
using TaskFlow.Application.Features.Boards.GetBoardById;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class BoardColumnsController : ControllerBase
{
    private readonly ISender _sender;

    public BoardColumnsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("boards/{boardId:guid}/columns")]
    [ProducesResponseType(typeof(IReadOnlyList<BoardColumnListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByBoard(
        Guid boardId,
        CancellationToken cancellationToken)
    {
        var board = await _sender.Send(
            new GetBoardByIdQuery(boardId),
            cancellationToken);

        if (board is null)
        {
            return NotFound(
                ApiProblemDetails.NotFound("Quadro não encontrado."));
        }

        var columns = await _sender.Send(
            new GetBoardColumnsQuery(boardId),
            cancellationToken);

        return Ok(columns);
    }

    [HttpGet("columns/{id:guid}")]
    [ProducesResponseType(typeof(BoardColumnDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var column = await _sender.Send(
            new GetBoardColumnByIdQuery(id),
            cancellationToken);

        return column is null
            ? NotFound(ApiProblemDetails.NotFound("Coluna não encontrada."))
            : Ok(column);
    }

    [HttpPost("boards/{boardId:guid}/columns")]
    [ProducesResponseType(typeof(IdResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(
        Guid boardId,
        [FromBody] CreateBoardColumnRequest request,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            new CreateBoardColumnCommand(boardId, request.Name),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new IdResponse(id));
    }

    [HttpPut("columns/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateBoardColumnRequest request,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new UpdateBoardColumnCommand(id, request.Name),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("columns/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteBoardColumnCommand(id),
            cancellationToken);

        return NoContent();
    }

    [HttpPut("boards/{boardId:guid}/columns/order")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reorder(
        Guid boardId,
        [FromBody] ReorderBoardColumnsRequest request,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new ReorderBoardColumnsCommand(boardId, request.ColumnIds),
            cancellationToken);

        return NoContent();
    }
}
