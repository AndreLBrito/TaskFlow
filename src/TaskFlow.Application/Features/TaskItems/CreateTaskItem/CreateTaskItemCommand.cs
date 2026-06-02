using MediatR;

namespace TaskFlow.Application.Features.TaskItems.CreateTaskItem;

public record CreateTaskItemCommand(
    Guid BoardColumnId,
    string? Title,
    string? Description
) : IRequest<Guid>;