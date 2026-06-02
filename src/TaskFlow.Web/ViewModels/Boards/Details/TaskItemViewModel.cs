namespace TaskFlow.Web.ViewModels.Boards.Details;

public class TaskItemViewModel
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }
}