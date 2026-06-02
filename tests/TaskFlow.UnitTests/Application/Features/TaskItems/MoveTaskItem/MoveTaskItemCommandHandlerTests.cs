using FluentAssertions;
using Moq;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Features.TaskItems.MoveTaskItem;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.UnitTests.Features.TaskItems.MoveTaskItem;

public class MoveTaskItemCommandHandlerTests
{
    private readonly Mock<ITaskItemRepository> _taskItemRepository;
    private readonly Mock<IBoardColumnRepository> _boardColumnRepository;

    private readonly MoveTaskItemCommandHandler _handler;

    public MoveTaskItemCommandHandlerTests()
    {
        _taskItemRepository = new Mock<ITaskItemRepository>();
        _boardColumnRepository = new Mock<IBoardColumnRepository>();

        _handler = new MoveTaskItemCommandHandler(
            _taskItemRepository.Object,
            _boardColumnRepository.Object);
    }

    [Fact]
    public async Task Handle_DeveMoverTarefaParaOutraColuna()
    {
        var sourceColumnId = Guid.NewGuid();
        var destinationColumnId = Guid.NewGuid();

        var taskItem = new TaskItem(
            sourceColumnId,
            "Tarefa teste");

        var destinationColumn = new BoardColumn(
            Guid.NewGuid(),
            "Em Andamento",
            1);

        _taskItemRepository
            .Setup(repository => repository.GetByIdAsync(
                taskItem.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskItem);

        _boardColumnRepository
            .Setup(repository => repository.GetByIdAsync(
                destinationColumnId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationColumn);

        _taskItemRepository
            .Setup(repository => repository.GetNextOrderAsync(
                destinationColumnId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(3);

        var command = new MoveTaskItemCommand(
            taskItem.Id,
            destinationColumnId,
            0);

        await _handler.Handle(
            command,
            CancellationToken.None);

        taskItem.BoardColumnId.Should()
            .Be(destinationColumnId);

        taskItem.Order.Should()
            .Be(3);

        _taskItemRepository.Verify(
            repository => repository.UpdateAsync(
                taskItem,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecaoQuandoTarefaNaoExistir()
    {
        _taskItemRepository
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null);

        var command = new MoveTaskItemCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            0);

        var action = async () => await _handler.Handle(
            command,
            CancellationToken.None);

        await action.Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_DeveLancarExcecaoQuandoColunaNaoExistir()
    {
        var taskItem = new TaskItem(
            Guid.NewGuid(),
            "Tarefa teste");

        _taskItemRepository
            .Setup(repository => repository.GetByIdAsync(
                taskItem.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskItem);

        _boardColumnRepository
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((BoardColumn?)null);

        var command = new MoveTaskItemCommand(
            taskItem.Id,
            Guid.NewGuid(),
            0);

        var action = async () => await _handler.Handle(
            command,
            CancellationToken.None);

        await action.Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldMoveTaskItemToAnotherColumn_WhenDataIsValid()
    {
        var sourceColumnId = Guid.NewGuid();
        var destinationColumn = new BoardColumn(
            Guid.NewGuid(),
            "Em Andamento",
            2);

        var taskItem = new TaskItem(
            sourceColumnId,
            "Tarefa teste",
            order: 0);

        var command = new MoveTaskItemCommand(
            taskItem.Id,
            destinationColumn.Id,
            0);

        _taskItemRepository
            .Setup(repository => repository.GetByIdAsync(
                taskItem.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskItem);

        _boardColumnRepository
            .Setup(repository => repository.GetByIdAsync(
                destinationColumn.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationColumn);

        _taskItemRepository
            .Setup(repository => repository.GetNextOrderAsync(
                destinationColumn.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(3);

        await _handler.Handle(
            command,
            CancellationToken.None);

        taskItem.BoardColumnId.Should().Be(destinationColumn.Id);
        taskItem.Order.Should().Be(3);

        _taskItemRepository.Verify(
            repository => repository.UpdateAsync(
                taskItem,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenTaskItemDoesNotExist()
    {
        var command = new MoveTaskItemCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            0);

        _taskItemRepository
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null);

        var action = async () => await _handler.Handle(
            command,
            CancellationToken.None);

        await action.Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenColumnDoesNotExist()
    {
        var taskItem = new TaskItem(
            Guid.NewGuid(),
            "Tarefa teste");

        var command = new MoveTaskItemCommand(
            taskItem.Id,
            Guid.NewGuid(),
            0);

        _taskItemRepository
            .Setup(repository => repository.GetByIdAsync(
                taskItem.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskItem);

        _boardColumnRepository
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((BoardColumn?)null);

        var action = async () => await _handler.Handle(
            command,
            CancellationToken.None);

        await action.Should()
            .ThrowAsync<NotFoundException>();

        _taskItemRepository.Verify(
            repository => repository.UpdateAsync(
                It.IsAny<TaskItem>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}