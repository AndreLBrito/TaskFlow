using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces;

public interface IWorkspaceRepository
{
    Task AddAsync(Workspace workspace, CancellationToken cancellationToken);

    Task<Workspace?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Workspace>> GetAllAsync(CancellationToken cancellationToken);

    Task UpdateAsync(
        Workspace workspace,
        CancellationToken cancellationToken);

    Task DeleteAsync(Workspace workspace, CancellationToken cancellationToken);

    Task<int> CountAsync(
        CancellationToken cancellationToken);
}