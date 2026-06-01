namespace TaskFlow.Web.ViewModels.Boards.Details;

public class BoardDetailsViewModel
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
}