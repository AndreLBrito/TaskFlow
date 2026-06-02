using FluentAssertions;
using Moq;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Features.Boards.CreateBoard;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.UnitTests.Features.Boards.CreateBoard;

public class CreateBoardCommandHandlerTests
{
    private readonly Mock<IWorkspaceRepository> _workspaceRepository;
    private readonly Mock<IBoardRepository> _boardRepository;
    private readonly Mock<IBoardColumnRepository> _boardColumnRepository;

    private readonly CreateBoardCommandHandler _handler;

    public CreateBoardCommandHandlerTests()
    {
        _workspaceRepository = new Mock<IWorkspaceRepository>();
        _boardRepository = new Mock<IBoardRepository>();
        _boardColumnRepository = new Mock<IBoardColumnRepository>();

        _handler = new CreateBoardCommandHandler(
            _workspaceRepository.Object,
            _boardRepository.Object,
            _boardColumnRepository.Object);
    }

    [Fact]
    public async Task Handle_DeveCriarQuadroEColunasPadrao()
    {
        var workspace = new Workspace("Workspace Teste");

        var command = new CreateBoardCommand(
            workspace.Id,
            "Quadro Teste",
            "Descrição");

        _workspaceRepository
            .Setup(repository => repository.GetByIdAsync(
                workspace.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(workspace);

        var boardId = await _handler.Handle(
            command,
            CancellationToken.None);

        boardId.Should().NotBeEmpty();

        _boardRepository.Verify(
            repository => repository.AddAsync(
                It.IsAny<Board>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _boardColumnRepository.Verify(
            repository => repository.AddRangeAsync(
                It.Is<IEnumerable<BoardColumn>>(columns =>
                    columns.Count() == 3),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecaoQuandoWorkspaceNaoExistir()
    {
        var command = new CreateBoardCommand(
            Guid.NewGuid(),
            "Quadro Teste",
            null);

        _workspaceRepository
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Workspace?)null);

        var action = async () => await _handler.Handle(
            command,
            CancellationToken.None);

        await action.Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_ShouldCreateBoardAndDefaultColumns_WhenWorkspaceExists()
    {
        var workspace = new Workspace("Workspace teste");

        var command = new CreateBoardCommand(
            workspace.Id,
            "Quadro teste",
            "Descrição");

        _workspaceRepository
            .Setup(repository => repository.GetByIdAsync(
                workspace.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(workspace);

        var boardId = await _handler.Handle(
            command,
            CancellationToken.None);

        boardId.Should().NotBeEmpty();

        _boardRepository.Verify(
            repository => repository.AddAsync(
                It.Is<Board>(board =>
                    board.Id == boardId &&
                    board.WorkspaceId == workspace.Id &&
                    board.Name == "Quadro teste"),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _boardColumnRepository.Verify(
            repository => repository.AddRangeAsync(
                It.Is<IEnumerable<BoardColumn>>(columns =>
                    columns.Count() == 3 &&
                    columns.Any(column => column.Name == "A Fazer") &&
                    columns.Any(column => column.Name == "Em Andamento") &&
                    columns.Any(column => column.Name == "Concluído")),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenWorkspaceDoesNotExist()
    {
        var command = new CreateBoardCommand(
            Guid.NewGuid(),
            "Quadro teste",
            null);

        _workspaceRepository
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Workspace?)null);

        var action = async () => await _handler.Handle(
            command,
            CancellationToken.None);

        await action.Should()
            .ThrowAsync<NotFoundException>();

        _boardRepository.Verify(
            repository => repository.AddAsync(
                It.IsAny<Board>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _boardColumnRepository.Verify(
            repository => repository.AddRangeAsync(
                It.IsAny<IEnumerable<BoardColumn>>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}