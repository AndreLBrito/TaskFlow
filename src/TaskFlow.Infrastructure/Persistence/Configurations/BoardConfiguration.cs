using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations;

public class BoardConfiguration : IEntityTypeConfiguration<Board>
{
    public void Configure(EntityTypeBuilder<Board> builder)
    {
        builder.ToTable("Boards");

        builder.HasKey(board => board.Id);

        builder.HasOne<Workspace>()
            .WithMany()
            .HasForeignKey(board => board.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(board => board.WorkspaceId)
            .IsRequired();

        builder.Property(board => board.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(board => board.Description)
            .HasMaxLength(500);

        builder.Property(board => board.CreatedAt)
            .IsRequired();

        builder.Property(board => board.UpdatedAt);

        builder.HasIndex(board => board.WorkspaceId);
    }
}