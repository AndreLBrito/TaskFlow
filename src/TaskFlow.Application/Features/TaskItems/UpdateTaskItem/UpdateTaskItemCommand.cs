using MediatR;

namespace TaskFlow.Application.Features.TaskItems.UpdateTaskItem;

public record UpdateTaskItemCommand(
    Guid Id,
    string? Title,
    string? Description,
    DateTime? DueDate
) : IRequest;