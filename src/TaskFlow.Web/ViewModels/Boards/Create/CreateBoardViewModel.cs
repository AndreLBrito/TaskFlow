namespace TaskFlow.Web.ViewModels.Boards.Create;

public class CreateBoardViewModel
{
    public Guid WorkspaceId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
}