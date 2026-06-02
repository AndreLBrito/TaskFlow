namespace TaskFlow.Web.ViewModels.TaskItems.Update;

public class UpdateTaskItemViewModel
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }
}