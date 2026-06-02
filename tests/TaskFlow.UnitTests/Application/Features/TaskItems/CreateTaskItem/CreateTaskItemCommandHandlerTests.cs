using FluentAssertions;
using Moq;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Features.TaskItems.CreateTaskItem;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Application.Features.TaskItems.CreateTaskItem;

public class CreateTaskItemCommandHandlerTests
{
    private readonly Mock<IBoardColumnRepository> _boardColumnRepositoryMock = new();
    private readonly Mock<ITaskItemRepository> _taskItemRepositoryMock = new();

    private readonly CreateTaskItemCommandHandler _handler;

    public CreateTaskItemCommandHandlerTests()
    {
        _handler = new CreateTaskItemCommandHandler(
            _boardColumnRepositoryMock.Object,
            _taskItemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateTaskItem_WhenColumnExists()
    {
        var boardId = Guid.NewGuid();
        var column = new BoardColumn(boardId, "A Fazer", 1);

        var command = new CreateTaskItemCommand(
            column.Id,
            "Tarefa teste",
            "Descrição");

        _boardColumnRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                column.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(column);

        _taskItemRepositoryMock
            .Setup(repository => repository.GetNextOrderAsync(
                column.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);

        var taskId = await _handler.Handle(
            command,
            CancellationToken.None);

        taskId.Should().NotBeEmpty();

        _taskItemRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.Is<TaskItem>(task =>
                    task.Id == taskId &&
                    task.BoardColumnId == column.Id &&
                    task.Title == "Tarefa teste" &&
                    task.Description == "Descrição" &&
                    task.Order == 2),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenColumnDoesNotExist()
    {
        var command = new CreateTaskItemCommand(
            Guid.NewGuid(),
            "Tarefa teste",
            null);

        _boardColumnRepositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((BoardColumn?)null);

        var action = async () => await _handler.Handle(
            command,
            CancellationToken.None);

        await action.Should()
            .ThrowAsync<NotFoundException>();

        _taskItemRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<TaskItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}