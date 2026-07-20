using FluentAssertions;
using Moq;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Features.TaskItems.MoveTaskItem;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Application.Features.TaskItems.MoveTaskItem;

public class MoveTaskItemCommandHandlerTests
{
    private readonly Mock<ITaskItemRepository> _taskItemRepository = new();
    private readonly Mock<IBoardColumnRepository> _boardColumnRepository = new();

    [Fact]
    public async Task Handle_ShouldMoveAndReorderTask_WhenDestinationIsValid()
    {
        var boardId = Guid.NewGuid();
        var sourceColumn = new BoardColumn(boardId, "A Fazer", 0);
        var targetColumn = new BoardColumn(boardId, "Em Andamento", 1);
        var movedTask = new TaskItem(sourceColumn.Id, "Mover", order: 0);
        var targetTask = new TaskItem(targetColumn.Id, "Existente", order: 0);

        SetupColumns(sourceColumn, targetColumn);

        _taskItemRepository
            .Setup(repository => repository.GetByIdAsync(
                movedTask.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(movedTask);

        _taskItemRepository
            .Setup(repository => repository.GetByColumnIdAsync(
                sourceColumn.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([movedTask]);

        _taskItemRepository
            .Setup(repository => repository.GetByColumnIdAsync(
                targetColumn.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([targetTask]);

        var handler = CreateHandler();

        await handler.Handle(
            new MoveTaskItemCommand(movedTask.Id, targetColumn.Id, 0),
            CancellationToken.None);

        movedTask.BoardColumnId.Should().Be(targetColumn.Id);
        movedTask.Order.Should().Be(0);
        targetTask.Order.Should().Be(1);

        _taskItemRepository.Verify(
            repository => repository.UpdateRangeAsync(
                It.Is<IEnumerable<TaskItem>>(tasks => tasks.Count() == 2),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReorderTaskWithinSameColumn()
    {
        var column = new BoardColumn(Guid.NewGuid(), "A Fazer", 0);
        var firstTask = new TaskItem(column.Id, "Primeira", order: 0);
        var movedTask = new TaskItem(column.Id, "Segunda", order: 1);

        _taskItemRepository
            .Setup(repository => repository.GetByIdAsync(
                movedTask.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(movedTask);

        _boardColumnRepository
            .Setup(repository => repository.GetByIdAsync(
                column.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(column);

        _taskItemRepository
            .Setup(repository => repository.GetByColumnIdAsync(
                column.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([firstTask, movedTask]);

        var handler = CreateHandler();

        await handler.Handle(
            new MoveTaskItemCommand(movedTask.Id, column.Id, 0),
            CancellationToken.None);

        movedTask.Order.Should().Be(0);
        firstTask.Order.Should().Be(1);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
    {
        _taskItemRepository
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null);

        var action = () => CreateHandler().Handle(
            new MoveTaskItemCommand(Guid.NewGuid(), Guid.NewGuid(), 0),
            CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenTargetColumnDoesNotExist()
    {
        var sourceColumn = new BoardColumn(Guid.NewGuid(), "A Fazer", 0);
        var task = new TaskItem(sourceColumn.Id, "Tarefa");
        var targetColumnId = Guid.NewGuid();

        _taskItemRepository
            .Setup(repository => repository.GetByIdAsync(
                task.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        _boardColumnRepository
            .Setup(repository => repository.GetByIdAsync(
                sourceColumn.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(sourceColumn);

        _boardColumnRepository
            .Setup(repository => repository.GetByIdAsync(
                targetColumnId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((BoardColumn?)null);

        var action = () => CreateHandler().Handle(
            new MoveTaskItemCommand(task.Id, targetColumnId, 0),
            CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldRejectMoveBetweenDifferentBoards()
    {
        var sourceColumn = new BoardColumn(Guid.NewGuid(), "A Fazer", 0);
        var targetColumn = new BoardColumn(Guid.NewGuid(), "Outro quadro", 0);
        var task = new TaskItem(sourceColumn.Id, "Tarefa");

        SetupColumns(sourceColumn, targetColumn);

        _taskItemRepository
            .Setup(repository => repository.GetByIdAsync(
                task.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        var action = () => CreateHandler().Handle(
            new MoveTaskItemCommand(task.Id, targetColumn.Id, 0),
            CancellationToken.None);

        await action.Should().ThrowAsync<BusinessRuleException>();

        _taskItemRepository.Verify(
            repository => repository.UpdateRangeAsync(
                It.IsAny<IEnumerable<TaskItem>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    private MoveTaskItemCommandHandler CreateHandler()
    {
        return new MoveTaskItemCommandHandler(
            _taskItemRepository.Object,
            _boardColumnRepository.Object);
    }

    private void SetupColumns(
        BoardColumn sourceColumn,
        BoardColumn targetColumn)
    {
        _boardColumnRepository
            .Setup(repository => repository.GetByIdAsync(
                sourceColumn.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(sourceColumn);

        _boardColumnRepository
            .Setup(repository => repository.GetByIdAsync(
                targetColumn.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(targetColumn);
    }
}
