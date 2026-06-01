namespace TaskFlow.Web.ViewModels.Boards.List;

public class BoardListViewModel
{
    public Guid WorkspaceId { get; set; }

    public IReadOnlyList<BoardListItemViewModel> Boards { get; set; }
        = [];
}