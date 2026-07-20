using MediatR;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.TaskItems.GetTasksByColumn;

public class GetTasksByColumnQueryHandler
    : IRequestHandler<GetTasksByColumnQuery, IReadOnlyList<TaskItemListItemDto>>
{
    private readonly ITaskItemRepository _taskItemRepository;

    public GetTasksByColumnQueryHandler(
        ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<IReadOnlyList<TaskItemListItemDto>> Handle(
        GetTasksByColumnQuery request,
        CancellationToken cancellationToken)
    {
        var tasks = await _taskItemRepository.GetByColumnIdAsync(
            request.BoardColumnId,
            cancellationToken);

        return tasks
            .Select(task => new TaskItemListItemDto
            {
                Id = task.Id,
                BoardColumnId = task.BoardColumnId,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Order = task.Order
            })
            .ToList();
    }
}
