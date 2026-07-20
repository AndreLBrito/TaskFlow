using MediatR;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

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

        var sourceColumn = await _boardColumnRepository.GetByIdAsync(
            taskItem.BoardColumnId,
            cancellationToken);

        if (sourceColumn is null)
        {
            throw new NotFoundException("Coluna de origem não encontrada.");
        }

        var targetColumn = await _boardColumnRepository.GetByIdAsync(
            request.BoardColumnId,
            cancellationToken);

        if (targetColumn is null)
        {
            throw new NotFoundException("Coluna não encontrada.");
        }

        if (sourceColumn.BoardId != targetColumn.BoardId)
        {
            throw new BusinessRuleException(
                "A tarefa não pode ser movida para uma coluna de outro quadro.");
        }

        var sourceTasks = (await _taskItemRepository.GetByColumnIdAsync(
                sourceColumn.Id,
                cancellationToken))
            .Where(task => task.Id != taskItem.Id)
            .ToList();

        var targetTasks = sourceColumn.Id == targetColumn.Id
            ? sourceTasks
            : (await _taskItemRepository.GetByColumnIdAsync(
                    targetColumn.Id,
                    cancellationToken))
                .Where(task => task.Id != taskItem.Id)
                .ToList();

        var targetOrder = Math.Min(request.Order, targetTasks.Count);
        targetTasks.Insert(targetOrder, taskItem);

        ReorderTasks(sourceTasks, sourceColumn.Id);
        ReorderTasks(targetTasks, targetColumn.Id);

        var changedTasks = sourceTasks
            .Concat(targetTasks)
            .DistinctBy(task => task.Id);

        await _taskItemRepository.UpdateRangeAsync(
            changedTasks,
            cancellationToken);
    }

    private static void ReorderTasks(
        IReadOnlyList<TaskItem> tasks,
        Guid boardColumnId)
    {
        for (var index = 0; index < tasks.Count; index++)
        {
            tasks[index].MoveToColumn(boardColumnId, index);
        }
    }
}
