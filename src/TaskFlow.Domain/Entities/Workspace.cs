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
        Name = name;
        Description = description;
    }
}