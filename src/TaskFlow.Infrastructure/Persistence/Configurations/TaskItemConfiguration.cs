using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("TaskItems");

        builder.HasOne<BoardColumn>()
            .WithMany()
            .HasForeignKey(taskItem => taskItem.BoardColumnId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasKey(taskItem => taskItem.Id);

        builder.Property(taskItem => taskItem.BoardColumnId)
            .IsRequired();

        builder.Property(taskItem => taskItem.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(taskItem => taskItem.Description)
            .HasMaxLength(1000);

        builder.Property(taskItem => taskItem.DueDate);

        builder.Property(taskItem => taskItem.Order)
            .IsRequired();

        builder.Property(taskItem => taskItem.CreatedAt)
            .IsRequired();

        builder.Property(taskItem => taskItem.UpdatedAt);

        builder.HasIndex(taskItem => taskItem.BoardColumnId);

        builder.HasIndex(taskItem => new { taskItem.BoardColumnId, taskItem.Order });
    }
}