using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Workspace> Workspaces => Set<Workspace>();
    public DbSet<Board> Boards => Set<Board>();
    public DbSet<BoardColumn> BoardColumns => Set<BoardColumn>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<TaskComment> TaskComments => Set<TaskComment>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}