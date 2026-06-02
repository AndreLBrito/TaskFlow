using MediatR;

namespace TaskFlow.Application.Features.TaskItems.GetTaskItemById;

public record GetTaskItemByIdQuery(Guid Id)
    : IRequest<TaskItemDetailsDto?>;