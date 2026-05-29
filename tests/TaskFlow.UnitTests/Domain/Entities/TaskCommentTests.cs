using FluentAssertions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Domain.Entities;

public class TaskCommentTests
{
    [Fact]
    public void Constructor_ShouldCreateComment_WhenDataIsValid()
    {
        var taskItemId = Guid.NewGuid();

        var comment = new TaskComment(taskItemId, "Primeiro comentário");

        comment.TaskItemId.Should().Be(taskItemId);
        comment.Content.Should().Be("Primeiro comentário");
        comment.Id.Should().NotBeEmpty();
        comment.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenTaskItemIdIsEmpty()
    {
        var action = () => new TaskComment(Guid.Empty, "Comentário");

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("A tarefa é obrigatória.*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenContentIsEmpty()
    {
        var action = () => new TaskComment(Guid.NewGuid(), "");

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("O comentário é obrigatório.*");
    }

    [Fact]
    public void UpdateContent_ShouldUpdateContentAndUpdatedAt_WhenContentIsValid()
    {
        var comment = new TaskComment(Guid.NewGuid(), "Texto antigo");

        comment.UpdateContent("Texto novo");

        comment.Content.Should().Be("Texto novo");
        comment.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void UpdateContent_ShouldThrowArgumentException_WhenContentIsEmpty()
    {
        var comment = new TaskComment(Guid.NewGuid(), "Texto inicial");

        var action = () => comment.UpdateContent("");

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("O comentário é obrigatório.*");
    }
}