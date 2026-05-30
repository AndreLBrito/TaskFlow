using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly AppDbContext _context;

    public WorkspaceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Workspace workspace, CancellationToken cancellationToken)
    {
        await _context.Workspaces.AddAsync(workspace, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Workspace?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Workspaces
            .FirstOrDefaultAsync(workspace => workspace.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Workspace>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Workspaces
            .OrderBy(workspace => workspace.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(
    Workspace workspace,
    CancellationToken cancellationToken)
    {
        _context.Workspaces.Update(workspace);

        await _context.SaveChangesAsync(cancellationToken);
    }
}