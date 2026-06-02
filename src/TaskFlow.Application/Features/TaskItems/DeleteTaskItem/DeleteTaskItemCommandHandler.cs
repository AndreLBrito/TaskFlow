using MediatR;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.TaskItems.DeleteTaskItem;

public class DeleteTaskItemCommandHandler
    : IRequestHandler<DeleteTaskItemCommand>
{
    private readonly ITaskItemRepository _taskItemRepository;

    public DeleteTaskItemCommandHandler(
        ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task Handle(
        DeleteTaskItemCommand request,
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

        await _taskItemRepository.DeleteAsync(
            taskItem,
            cancellationToken);
    }
}