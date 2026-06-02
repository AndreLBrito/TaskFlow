using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class TaskItem : BaseEntity
{
    public Guid BoardColumnId { get; private set; }

    public BoardColumn? BoardColumn { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public DateTime? DueDate { get; private set; }

    public int Order { get; private set; }

    private TaskItem()
    {
    }

    public TaskItem(Guid boardColumnId, string title, string? description = null, DateTime? dueDate = null, int order = 0)
    {
        if (boardColumnId == Guid.Empty)
        {
            throw new ArgumentException("A coluna do quadro é obrigatória.", nameof(boardColumnId));
        }

        if (order < 0)
        {
            throw new ArgumentException("A ordem da tarefa não pode ser negativa.", nameof(order));
        }

        SetTitle(title);

        BoardColumnId = boardColumnId;
        Description = description;
        DueDate = dueDate;
        Order = order;
    }

    public void Rename(string title)
    {
        SetTitle(title);
        SetUpdatedAt();
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
        SetUpdatedAt();
    }

    public void ChangeDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;
        SetUpdatedAt();
    }

    public void MoveToColumn(Guid boardColumnId, int order)
    {
        if (boardColumnId == Guid.Empty)
        {
            throw new ArgumentException("A coluna do quadro é obrigatória.", nameof(boardColumnId));
        }

        if (order < 0)
        {
            throw new ArgumentException("A ordem da tarefa não pode ser negativa.", nameof(order));
        }

        BoardColumnId = boardColumnId;
        Order = order;
        SetUpdatedAt();
    }

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("O título da tarefa é obrigatório.", nameof(title));
        }

        if (title.Length > 150)
        {
            throw new ArgumentException("O título da tarefa não pode ter mais de 150 caracteres.", nameof(title));
        }

        Title = title.Trim();
    }
}