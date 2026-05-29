using FluentAssertions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Domain.Entities;

public class BoardTests
{
    [Fact]
    public void Constructor_ShouldCreateBoard_WhenDataIsValid()
    {
        var workspaceId = Guid.NewGuid();

        var board = new Board(workspaceId, "Quadro principal", "Descrição do quadro");

        board.WorkspaceId.Should().Be(workspaceId);
        board.Name.Should().Be("Quadro principal");
        board.Description.Should().Be("Descrição do quadro");
        board.Id.Should().NotBeEmpty();
        board.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenWorkspaceIdIsEmpty()
    {
        var action = () => new Board(Guid.Empty, "Quadro principal");

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("O workspace é obrigatório.*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        var action = () => new Board(Guid.NewGuid(), "");

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("O nome do quadro é obrigatório.*");
    }

    [Fact]
    public void Rename_ShouldUpdateNameAndUpdatedAt_WhenNameIsValid()
    {
        var board = new Board(Guid.NewGuid(), "Nome antigo");

        board.Rename("Nome novo");

        board.Name.Should().Be("Nome novo");
        board.UpdatedAt.Should().NotBeNull();
    }
}