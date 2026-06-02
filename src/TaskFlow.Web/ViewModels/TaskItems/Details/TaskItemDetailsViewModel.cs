namespace TaskFlow.Web.ViewModels.TaskItems.Details;

public class TaskItemDetailsViewModel
{
    public Guid Id { get; set; }

    public Guid BoardColumnId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }
}