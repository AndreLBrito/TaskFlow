using MediatR;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.TaskItems.MoveTaskItem;

public class MoveTaskItemCommandHandler
    : IRequestHandler<MoveTaskItemCommand>
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IBoardColumnRepository _boardColumnRepository;

    public MoveTaskItemCommandHandler(
        ITaskItemRepository taskItemRepository,
        IBoardColumnRepository boardColumnRepository)
    {
        _taskItemRepository = taskItemRepository;
        _boardColumnRepository = boardColumnRepository;
    }

    public async Task Handle(
        MoveTaskItemCommand request,
        CancellationToken cancellationToken)
    {
        var taskItem = await _taskItemRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (taskItem is null)
        {
            throw new NotFoundException("Tarefa não encontrada.");
        }

        var boardColumn = await _boardColumnRepository.GetByIdAsync(
            request.BoardColumnId,
            cancellationToken);

        if (boardColumn is null)
        {
            throw new NotFoundException("Coluna não encontrada.");
        }

        var order = await _taskItemRepository.GetNextOrderAsync(
            request.BoardColumnId,
            cancellationToken);

        taskItem.MoveToColumn(
            request.BoardColumnId,
            order);

        await _taskItemRepository.UpdateAsync(
            taskItem,
            cancellationToken);
    }
}