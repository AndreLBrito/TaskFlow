namespace TaskFlow.Application.Features.BoardColumns.GetBoardColumns;

public class BoardColumnListItemDto
{
    public Guid Id { get; set; }

    public Guid BoardId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Order { get; set; }
}