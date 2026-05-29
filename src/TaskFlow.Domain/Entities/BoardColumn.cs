using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class BoardColumn : BaseEntity
{
    public Guid BoardId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public int Order { get; private set; }

    private BoardColumn()
    {
    }

    public BoardColumn(Guid boardId, string name, int order)
    {
        if (boardId == Guid.Empty)
        {
            throw new ArgumentException("O quadro é obrigatório.", nameof(boardId));
        }

        if (order < 0)
        {
            throw new ArgumentException("A ordem da coluna não pode ser negativa.", nameof(order));
        }

        SetName(name);

        BoardId = boardId;
        Order = order;
    }

    public void Rename(string name)
    {
        SetName(name);
        SetUpdatedAt();
    }

    public void ChangeOrder(int order)
    {
        if (order < 0)
        {
            throw new ArgumentException("A ordem da coluna não pode ser negativa.", nameof(order));
        }

        Order = order;
        SetUpdatedAt();
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("O nome da coluna é obrigatório.", nameof(name));
        }

        if (name.Length > 100)
        {
            throw new ArgumentException("O nome da coluna não pode ter mais de 100 caracteres.", nameof(name));
        }

        Name = name.Trim();
    }
}