using MediatR;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Features.TaskItems.CreateTaskItem;

public class CreateTaskItemCommandHandler
    : IRequestHandler<CreateTaskItemCommand, Guid>
{
    private readonly IBoardColumnRepository _boardColumnRepository;
    private readonly ITaskItemRepository _taskItemRepository;

    public CreateTaskItemCommandHandler(
        IBoardColumnRepository boardColumnRepository,
        ITaskItemRepository taskItemRepository)
    {
        _boardColumnRepository = boardColumnRepository;
        _taskItemRepository = taskItemRepository;
    }

    public async Task<Guid> Handle(
        CreateTaskItemCommand request,
        CancellationToken cancellationToken)
    {
        var column = await _boardColumnRepository.GetByIdAsync(
            request.BoardColumnId,
            cancellationToken);

        if (column is null)
        {
            throw new NotFoundException("Coluna não encontrada.");
        }

        var tasks = await _taskItemRepository.GetByColumnIdAsync(
            request.BoardColumnId,
            cancellationToken);

        var order = tasks.Count + 1;

        var taskItem = new TaskItem(
            request.BoardColumnId,
            request.Title!,
            request.Description,
            null,
            order);

        await _taskItemRepository.AddAsync(
            taskItem,
            cancellationToken);

        return taskItem.Id;
    }
}