using MediatR;

namespace TaskFlow.Application.Features.TaskItems.DeleteTaskItem;

public record DeleteTaskItemCommand(Guid Id)
    : IRequest;