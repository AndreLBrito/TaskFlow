using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces;

public interface ITaskItemRepository
{
    Task AddAsync(
        TaskItem taskItem,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        TaskItem taskItem,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        TaskItem taskItem,
        CancellationToken cancellationToken);

    Task<TaskItem?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<TaskItem>> GetByColumnIdAsync(
        Guid boardColumnId,
        CancellationToken cancellationToken);

    Task<TaskItem?> GetByIdWithBoardAsync(
        Guid id,
        CancellationToken cancellationToken);
}