using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class Workspace : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    private Workspace()
    {
    }

    public Workspace(string name, string? description = null)
    {
        SetName(name);
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
            throw new ArgumentException("O nome do workspace é obrigatório.", nameof(name));
        }

        if (name.Length > 100)
        {
            throw new ArgumentException("O nome do workspace não pode ter mais de 100 caracteres.", nameof(name));
        }

        Name = name.Trim();
    }
}