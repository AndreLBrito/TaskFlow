namespace TaskFlow.Web.ViewModels.Boards.Update;

public class UpdateBoardViewModel
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
}