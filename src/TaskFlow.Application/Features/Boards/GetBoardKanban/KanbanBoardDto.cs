namespace TaskFlow.Application.Features.Boards.GetBoardKanban;

public class KanbanBoardDto
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public IReadOnlyList<KanbanColumnDto> Columns { get; set; } = [];
}

public class KanbanColumnDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Order { get; set; }

    public IReadOnlyList<KanbanTaskItemDto> Tasks { get; set; } = [];
}

public class KanbanTaskItemDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public int Order { get; set; }
}
