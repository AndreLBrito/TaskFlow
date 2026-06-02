namespace TaskFlow.Web.ViewModels.Boards.Details;

public class BoardColumnViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Order { get; set; }

    public IReadOnlyList<TaskItemViewModel> Tasks { get; set; } = [];
}