using MediatR;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.TaskItems.UpdateTaskItem;

public class UpdateTaskItemCommandHandler
    : IRequestHandler<UpdateTaskItemCommand>
{
    private readonly ITaskItemRepository _taskItemRepository;

    public UpdateTaskItemCommandHandler(
        ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task Handle(
        UpdateTaskItemCommand request,
        CancellationToken cancellationToken)
    {
        var taskItem = await _taskItemRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (taskItem is null)
        {
            throw new NotFoundException(
                "Tarefa não encontrada.");
        }

        taskItem.Rename(request.Title!);
        taskItem.UpdateDescription(request.Description);
        taskItem.ChangeDueDate(request.DueDate);

        await _taskItemRepository.UpdateAsync(
            taskItem,
            cancellationToken);
    }
}