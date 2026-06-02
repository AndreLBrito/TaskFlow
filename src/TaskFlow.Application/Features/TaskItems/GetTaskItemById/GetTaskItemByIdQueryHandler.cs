using MediatR;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.TaskItems.GetTaskItemById;

public class GetTaskItemByIdQueryHandler
    : IRequestHandler<GetTaskItemByIdQuery, TaskItemDetailsDto?>
{
    private readonly ITaskItemRepository _taskItemRepository;

    public GetTaskItemByIdQueryHandler(
        ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<TaskItemDetailsDto?> Handle(
        GetTaskItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        var task = await _taskItemRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (task is null)
        {
            return null;
        }

        return new TaskItemDetailsDto
        {
            Id = task.Id,
            BoardColumnId = task.BoardColumnId,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Order = task.Order
        };
    }
}