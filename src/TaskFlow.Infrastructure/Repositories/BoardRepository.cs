using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly AppDbContext _context;

    public BoardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Board board,
        CancellationToken cancellationToken)
    {
        await _context.Boards.AddAsync(board, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Board?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Boards
            .FirstOrDefaultAsync(board => board.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Board>> GetByWorkspaceIdAsync(
        Guid workspaceId,
        CancellationToken cancellationToken)
    {
        return await _context.Boards
            .Where(board => board.WorkspaceId == workspaceId)
            .OrderBy(board => board.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(
    Board board,
    CancellationToken cancellationToken)
    {
        _context.Boards.Update(board);

        await _context.SaveChangesAsync(cancellationToken);
    }
}