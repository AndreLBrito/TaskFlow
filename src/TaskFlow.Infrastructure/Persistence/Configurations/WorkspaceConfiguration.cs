using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations;

public class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.ToTable("Workspaces");

        builder.HasKey(workspace => workspace.Id);

        builder.Property(workspace => workspace.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(workspace => workspace.Description)
            .HasMaxLength(500);

        builder.Property(workspace => workspace.CreatedAt)
            .IsRequired();

        builder.Property(workspace => workspace.UpdatedAt);
    }
}