using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces;

public interface IWorkspaceRepository
{
    Task AddAsync(Workspace workspace, CancellationToken cancellationToken);

    Task<Workspace?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}