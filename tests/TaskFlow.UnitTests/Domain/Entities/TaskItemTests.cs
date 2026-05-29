using FluentAssertions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Domain.Entities;

public class TaskItemTests
{
    [Fact]
    public void Constructor_ShouldCreateTaskItem_WhenDataIsValid()
    {
        var columnId = Guid.NewGuid();
        var dueDate = DateTime.UtcNow.AddDays(3);

        var taskItem = new TaskItem(columnId, "Minha tarefa", "Descrição", dueDate, 1);

        taskItem.BoardColumnId.Should().Be(columnId);
        taskItem.Title.Should().Be("Minha tarefa");
        taskItem.Description.Should().Be("Descrição");
        taskItem.DueDate.Should().Be(dueDate);
        taskItem.Order.Should().Be(1);
        taskItem.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenBoardColumnIdIsEmpty()
    {
        var action = () => new TaskItem(Guid.Empty, "Minha tarefa");

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("A coluna do quadro é obrigatória.*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenTitleIsEmpty()
    {
        var action = () => new TaskItem(Guid.NewGuid(), "");

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("O título da tarefa é obrigatório.*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenOrderIsNegative()
    {
        var action = () => new TaskItem(Guid.NewGuid(), "Minha tarefa", order: -1);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("A ordem da tarefa não pode ser negativa.*");
    }

    [Fact]
    public void Rename_ShouldUpdateTitleAndUpdatedAt_WhenTitleIsValid()
    {
        var taskItem = new TaskItem(Guid.NewGuid(), "Título antigo");

        taskItem.Rename("Título novo");

        taskItem.Title.Should().Be("Título novo");
        taskItem.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void MoveToColumn_ShouldUpdateColumnOrderAndUpdatedAt_WhenDataIsValid()
    {
        var taskItem = new TaskItem(Guid.NewGuid(), "Minha tarefa");
        var newColumnId = Guid.NewGuid();

        taskItem.MoveToColumn(newColumnId, 2);

        taskItem.BoardColumnId.Should().Be(newColumnId);
        taskItem.Order.Should().Be(2);
        taskItem.UpdatedAt.Should().NotBeNull();
    }
}