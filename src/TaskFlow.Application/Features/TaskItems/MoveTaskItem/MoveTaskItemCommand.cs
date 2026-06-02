using MediatR;

namespace TaskFlow.Application.Features.TaskItems.MoveTaskItem;

public record MoveTaskItemCommand(
    Guid Id,
    Guid BoardColumnId,
    int Order
) : IRequest;