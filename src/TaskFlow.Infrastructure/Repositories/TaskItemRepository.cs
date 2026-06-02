using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly AppDbContext _context;

    public TaskItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        TaskItem taskItem,
        CancellationToken cancellationToken)
    {
        await _context.TaskItems.AddAsync(
            taskItem,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        TaskItem taskItem,
        CancellationToken cancellationToken)
    {
        _context.TaskItems.Update(taskItem);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        TaskItem taskItem,
        CancellationToken cancellationToken)
    {
        _context.TaskItems.Remove(taskItem);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<TaskItem?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.TaskItems
            .FirstOrDefaultAsync(
                task => task.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<TaskItem>> GetByColumnIdAsync(
        Guid boardColumnId,
        CancellationToken cancellationToken)
    {
        return await _context.TaskItems
            .Where(task => task.BoardColumnId == boardColumnId)
            .OrderBy(task => task.Order)
            .ToListAsync(cancellationToken);
    }

    public async Task<TaskItem?> GetByIdWithBoardAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.TaskItems
            .Include(task => task.BoardColumn)
            .ThenInclude(column => column!.Board)
            .FirstOrDefaultAsync(
                task => task.Id == id,
                cancellationToken);
    }

    public async Task<int> GetNextOrderAsync(
        Guid boardColumnId,
        CancellationToken cancellationToken)
    {
        var maxOrder = await _context.TaskItems
            .Where(task => task.BoardColumnId == boardColumnId)
            .Select(task => (int?)task.Order)
            .MaxAsync(cancellationToken);

        return (maxOrder ?? -1) + 1;
    }

    public async Task<int> CountAsync(
        CancellationToken cancellationToken)
    {
        return await _context.TaskItems
            .CountAsync(cancellationToken);
    }

    public async Task<int> CountCompletedAsync(
        CancellationToken cancellationToken)
    {
        return await _context.TaskItems
            .Include(task => task.BoardColumn)
            .CountAsync(
                task => task.BoardColumn!.Name == "Concluído",
                cancellationToken);
    }
}