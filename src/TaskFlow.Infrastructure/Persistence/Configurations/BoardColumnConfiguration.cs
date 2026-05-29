using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations;

public class BoardColumnConfiguration : IEntityTypeConfiguration<BoardColumn>
{
    public void Configure(EntityTypeBuilder<BoardColumn> builder)
    {
        builder.ToTable("BoardColumns");

        builder.HasOne<Board>()
            .WithMany()
            .HasForeignKey(column => column.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasKey(column => column.Id);

        builder.Property(column => column.BoardId)
            .IsRequired();

        builder.Property(column => column.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(column => column.Order)
            .IsRequired();

        builder.Property(column => column.CreatedAt)
            .IsRequired();

        builder.Property(column => column.UpdatedAt);
    }
}