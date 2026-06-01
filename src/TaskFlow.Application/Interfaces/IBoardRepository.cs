using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces;

public interface IBoardRepository
{
    Task AddAsync(
        Board board,
        CancellationToken cancellationToken);

    Task<Board?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<Board>> GetByWorkspaceIdAsync(
        Guid workspaceId,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Board board,
        CancellationToken cancellationToken);
}