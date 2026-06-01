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
}