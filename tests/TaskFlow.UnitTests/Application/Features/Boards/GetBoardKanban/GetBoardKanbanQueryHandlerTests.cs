using FluentAssertions;
using Moq;
using TaskFlow.Application.Features.Boards.GetBoardKanban;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Application.Features.Boards.GetBoardKanban;

public class GetBoardKanbanQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnColumnsAndTasksInOrder()
    {
        var board = new Board(Guid.NewGuid(), "Quadro");
        var laterColumn = new BoardColumn(board.Id, "Depois", 1);
        var firstColumn = new BoardColumn(board.Id, "Antes", 0);
        var laterTask = new TaskItem(firstColumn.Id, "Depois", order: 1);
        var firstTask = new TaskItem(firstColumn.Id, "Antes", order: 0);

        firstColumn.Tasks.Add(laterTask);
        firstColumn.Tasks.Add(firstTask);
        board.Columns.Add(laterColumn);
        board.Columns.Add(firstColumn);

        var repository = new Mock<IBoardRepository>();
        repository
            .Setup(item => item.GetByIdWithColumnsAndTasksAsync(
                board.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);

        var handler = new GetBoardKanbanQueryHandler(repository.Object);

        var result = await handler.Handle(
            new GetBoardKanbanQuery(board.Id),
            CancellationToken.None);

        result.Should().NotBeNull();
        result!.Columns.Select(column => column.Id)
            .Should().ContainInOrder(firstColumn.Id, laterColumn.Id);
        result.Columns[0].Tasks.Select(task => task.Id)
            .Should().ContainInOrder(firstTask.Id, laterTask.Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenBoardDoesNotExist()
    {
        var repository = new Mock<IBoardRepository>();
        repository
            .Setup(item => item.GetByIdWithColumnsAndTasksAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Board?)null);

        var handler = new GetBoardKanbanQueryHandler(repository.Object);

        var result = await handler.Handle(
            new GetBoardKanbanQuery(Guid.NewGuid()),
            CancellationToken.None);

        result.Should().BeNull();
    }
}
