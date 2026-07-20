using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories;

public class BoardColumnRepository : IBoardColumnRepository
{
    private readonly AppDbContext _context;

    public BoardColumnRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        BoardColumn boardColumn,
        CancellationToken cancellationToken)
    {
        await _context.BoardColumns.AddAsync(
            boardColumn,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(
        IEnumerable<BoardColumn> boardColumns,
        CancellationToken cancellationToken)
    {
        await _context.BoardColumns.AddRangeAsync(
            boardColumns,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BoardColumn>> GetByBoardIdAsync(
        Guid boardId,
        CancellationToken cancellationToken)
    {
        return await _context.BoardColumns
            .Where(column => column.BoardId == boardId)
            .OrderBy(column => column.Order)
            .ToListAsync(cancellationToken);
    }

    public async Task<BoardColumn?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.BoardColumns
            .FirstOrDefaultAsync(
                column => column.Id == id,
                cancellationToken);
    }

    public async Task<int> GetNextOrderAsync(
        Guid boardId,
        CancellationToken cancellationToken)
    {
        var maxOrder = await _context.BoardColumns
            .Where(column => column.BoardId == boardId)
            .Select(column => (int?)column.Order)
            .MaxAsync(cancellationToken);

        return (maxOrder ?? -1) + 1;
    }

    public async Task UpdateAsync(
        BoardColumn boardColumn,
        CancellationToken cancellationToken)
    {
        _context.BoardColumns.Update(boardColumn);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(
        IEnumerable<BoardColumn> boardColumns,
        CancellationToken cancellationToken)
    {
        _context.BoardColumns.UpdateRange(boardColumns);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        BoardColumn boardColumn,
        CancellationToken cancellationToken)
    {
        _context.BoardColumns.Remove(boardColumn);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
