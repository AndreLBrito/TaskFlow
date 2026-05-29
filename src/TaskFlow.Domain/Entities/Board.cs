using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class Board : BaseEntity
{
    public Guid WorkspaceId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    private Board()
    {
    }

    public Board(Guid workspaceId, string name, string? description = null)
    {
        if (workspaceId == Guid.Empty)
        {
            throw new ArgumentException("O workspace é obrigatório.", nameof(workspaceId));
        }

        SetName(name);

        WorkspaceId = workspaceId;
        Description = description;
    }

    public void Rename(string name)
    {
        SetName(name);
        SetUpdatedAt();
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
        SetUpdatedAt();
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("O nome do quadro é obrigatório.", nameof(name));
        }

        if (name.Length > 100)
        {
            throw new ArgumentException("O nome do quadro não pode ter mais de 100 caracteres.", nameof(name));
        }

        Name = name.Trim();
    }
}