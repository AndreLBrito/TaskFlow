namespace TaskFlow.Application.Features.TaskItems.GetTasksByColumn;

public class TaskItemListItemDto
{
    public Guid Id { get; set; }

    public Guid BoardColumnId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public int Order { get; set; }
}