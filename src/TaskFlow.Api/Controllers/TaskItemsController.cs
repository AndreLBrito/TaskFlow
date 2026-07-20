using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Contracts.Common;
using TaskFlow.Api.Contracts.TaskItems;
using TaskFlow.Application.Features.BoardColumns.GetBoardColumnById;
using TaskFlow.Application.Features.TaskItems.CreateTaskItem;
using TaskFlow.Application.Features.TaskItems.DeleteTaskItem;
using TaskFlow.Application.Features.TaskItems.GetTaskItemById;
using TaskFlow.Application.Features.TaskItems.GetTasksByColumn;
using TaskFlow.Application.Features.TaskItems.MoveTaskItem;
using TaskFlow.Application.Features.TaskItems.UpdateTaskItem;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class TaskItemsController : ControllerBase
{
    private readonly ISender _sender;

    public TaskItemsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("columns/{columnId:guid}/tasks")]
    [ProducesResponseType(typeof(IReadOnlyList<TaskItemListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByColumn(
        Guid columnId,
        CancellationToken cancellationToken)
    {
        var column = await _sender.Send(
            new GetBoardColumnByIdQuery(columnId),
            cancellationToken);

        if (column is null)
        {
            return NotFound(
                ApiProblemDetails.NotFound("Coluna não encontrada."));
        }

        var tasks = await _sender.Send(
            new GetTasksByColumnQuery(columnId),
            cancellationToken);

        return Ok(tasks);
    }

    [HttpGet("tasks/{id:guid}")]
    [ProducesResponseType(typeof(TaskItemDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var task = await _sender.Send(
            new GetTaskItemByIdQuery(id),
            cancellationToken);

        return task is null
            ? NotFound(ApiProblemDetails.NotFound("Tarefa não encontrada."))
            : Ok(task);
    }

    [HttpPost("columns/{columnId:guid}/tasks")]
    [ProducesResponseType(typeof(IdResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(
        Guid columnId,
        [FromBody] CreateTaskItemRequest request,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            new CreateTaskItemCommand(
                columnId,
                request.Title,
                request.Description)
            {
                DueDate = request.DueDate
            },
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new IdResponse(id));
    }

    [HttpPut("tasks/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTaskItemRequest request,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new UpdateTaskItemCommand(
                id,
                request.Title,
                request.Description,
                request.DueDate),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("tasks/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteTaskItemCommand(id),
            cancellationToken);

        return NoContent();
    }

    [HttpPut("tasks/{id:guid}/move")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Move(
        Guid id,
        [FromBody] MoveTaskItemRequest request,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new MoveTaskItemCommand(
                id,
                request.TargetColumnId,
                request.Order),
            cancellationToken);

        return NoContent();
    }
}
