namespace TaskFlow.Web.ViewModels.TaskItems.Create;

public class CreateTaskItemViewModel
{
    public Guid BoardId { get; set; }

    public Guid BoardColumnId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }
}