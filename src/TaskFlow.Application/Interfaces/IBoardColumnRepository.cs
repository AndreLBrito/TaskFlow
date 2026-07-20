using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces;

public interface IBoardColumnRepository
{
    Task AddAsync(
        BoardColumn boardColumn,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IEnumerable<BoardColumn> boardColumns,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<BoardColumn>> GetByBoardIdAsync(
        Guid boardId,
        CancellationToken cancellationToken);

    Task<BoardColumn?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<int> GetNextOrderAsync(
        Guid boardId,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        BoardColumn boardColumn,
        CancellationToken cancellationToken);

    Task UpdateRangeAsync(
        IEnumerable<BoardColumn> boardColumns,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        BoardColumn boardColumn,
        CancellationToken cancellationToken);
}
