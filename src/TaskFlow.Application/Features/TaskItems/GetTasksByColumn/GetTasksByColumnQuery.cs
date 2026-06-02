using MediatR;

namespace TaskFlow.Application.Features.TaskItems.GetTasksByColumn;

public record GetTasksByColumnQuery(
    Guid BoardColumnId)
    : IRequest<IReadOnlyList<TaskItemListItemDto>>;