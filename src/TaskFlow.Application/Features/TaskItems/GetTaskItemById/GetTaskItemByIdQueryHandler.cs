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
        var task = await _taskItemRepository.GetByIdWithBoardAsync(
            request.Id,
            cancellationToken);

        if (task is null)
        {
            return null;
        }

        if (task.BoardColumn?.Board is null)
        {
            throw new InvalidOperationException(
                "Não foi possível identificar o quadro da tarefa.");
        }

        return new TaskItemDetailsDto
        {
            Id = task.Id,
            BoardId = task.BoardColumn!.Board!.Id,
            BoardColumnId = task.BoardColumnId,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Order = task.Order
        };
    }
}