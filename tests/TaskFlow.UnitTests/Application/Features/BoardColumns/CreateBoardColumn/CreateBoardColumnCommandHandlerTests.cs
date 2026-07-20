using FluentAssertions;
using Moq;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Features.BoardColumns.CreateBoardColumn;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Application.Features.BoardColumns.CreateBoardColumn;

public class CreateBoardColumnCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateColumnAtEnd_WhenBoardExists()
    {
        var board = new Board(Guid.NewGuid(), "Quadro");
        var boardRepository = new Mock<IBoardRepository>();
        var columnRepository = new Mock<IBoardColumnRepository>();

        boardRepository
            .Setup(repository => repository.GetByIdAsync(
                board.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(board);

        columnRepository
            .Setup(repository => repository.GetNextOrderAsync(
                board.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(3);

        var handler = new CreateBoardColumnCommandHandler(
            boardRepository.Object,
            columnRepository.Object);

        var id = await handler.Handle(
            new CreateBoardColumnCommand(board.Id, "Revisão"),
            CancellationToken.None);

        id.Should().NotBeEmpty();
        columnRepository.Verify(
            repository => repository.AddAsync(
                It.Is<BoardColumn>(column =>
                    column.Id == id &&
                    column.BoardId == board.Id &&
                    column.Name == "Revisão" &&
                    column.Order == 3),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenBoardDoesNotExist()
    {
        var boardRepository = new Mock<IBoardRepository>();
        var columnRepository = new Mock<IBoardColumnRepository>();

        boardRepository
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Board?)null);

        var handler = new CreateBoardColumnCommandHandler(
            boardRepository.Object,
            columnRepository.Object);

        var action = () => handler.Handle(
            new CreateBoardColumnCommand(Guid.NewGuid(), "Revisão"),
            CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }
}
