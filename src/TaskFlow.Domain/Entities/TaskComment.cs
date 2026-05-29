using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class TaskComment : BaseEntity
{
    public Guid TaskItemId { get; private set; }

    public string Content { get; private set; } = string.Empty;

    private TaskComment()
    {
    }

    public TaskComment(Guid taskItemId, string content)
    {
        if (taskItemId == Guid.Empty)
        {
            throw new ArgumentException("A tarefa é obrigatória.", nameof(taskItemId));
        }

        SetContent(content);

        TaskItemId = taskItemId;
    }

    public void UpdateContent(string content)
    {
        SetContent(content);
        SetUpdatedAt();
    }

    private void SetContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("O comentário é obrigatório.", nameof(content));
        }

        if (content.Length > 1000)
        {
            throw new ArgumentException("O comentário não pode ter mais de 1000 caracteres.", nameof(content));
        }

        Content = content.Trim();
    }
}