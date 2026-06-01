namespace TaskFlow.Application.Features.Boards.GetBoards;

public class BoardListItemDto
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
}