using FluentAssertions;
using Moq;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Features.BoardColumns.ReorderBoardColumns;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Application.Features.BoardColumns.ReorderBoardColumns;

public class ReorderBoardColumnsCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPersistRequestedColumnOrder()
    {
        var board = new Board(Guid.NewGuid(), "Quadro");
        var first = new BoardColumn(board.Id, "Primeira", 0);
        var second = new BoardColumn(board.Id, "Segunda", 1);
        var boardRepository = new Mock<IBoardRepository>();
        var columnRepository = new Mock<IBoardColumnRepository>();

        boardRepository
            .Setup(repository => repository.GetByIdAsync(
                board.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);

        columnRepository
            .Setup(repository => repository.GetByBoardIdAsync(
                board.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([first, second]);

        var handler = new ReorderBoardColumnsCommandHandler(
            boardRepository.Object,
            columnRepository.Object);

        await handler.Handle(
            new ReorderBoardColumnsCommand(board.Id, [second.Id, first.Id]),
            CancellationToken.None);

        second.Order.Should().Be(0);
        first.Order.Should().Be(1);
        columnRepository.Verify(
            repository => repository.UpdateRangeAsync(
                It.IsAny<IEnumerable<BoardColumn>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldRejectIncompleteColumnList()
    {
        var board = new Board(Guid.NewGuid(), "Quadro");
        var first = new BoardColumn(board.Id, "Primeira", 0);
        var second = new BoardColumn(board.Id, "Segunda", 1);
        var boardRepository = new Mock<IBoardRepository>();
        var columnRepository = new Mock<IBoardColumnRepository>();

        boardRepository
            .Setup(repository => repository.GetByIdAsync(
                board.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);

        columnRepository
            .Setup(repository => repository.GetByBoardIdAsync(
                board.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([first, second]);

        var handler = new ReorderBoardColumnsCommandHandler(
            boardRepository.Object,
            columnRepository.Object);

        var action = () => handler.Handle(
            new ReorderBoardColumnsCommand(board.Id, [first.Id]),
            CancellationToken.None);

        await action.Should().ThrowAsync<BusinessRuleException>();
    }
}
