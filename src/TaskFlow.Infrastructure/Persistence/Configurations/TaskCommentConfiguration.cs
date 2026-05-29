using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations;

public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> builder)
    {
        builder.ToTable("TaskComments");

        builder.HasOne<TaskItem>()
            .WithMany()
            .HasForeignKey(comment => comment.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasKey(comment => comment.Id);

        builder.Property(comment => comment.TaskItemId)
            .IsRequired();

        builder.Property(comment => comment.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(comment => comment.CreatedAt)
            .IsRequired();

        builder.Property(comment => comment.UpdatedAt);

        builder.HasIndex(comment => comment.TaskItemId);
    }
}