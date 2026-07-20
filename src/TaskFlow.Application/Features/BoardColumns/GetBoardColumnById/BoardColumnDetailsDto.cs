namespace TaskFlow.Application.Features.BoardColumns.GetBoardColumnById;

public class BoardColumnDetailsDto
{
    public Guid Id { get; set; }

    public Guid BoardId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Order { get; set; }

    public DateTime CreatedAt { get; set; }
}
