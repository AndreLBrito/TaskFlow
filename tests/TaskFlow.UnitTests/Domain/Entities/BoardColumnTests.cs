using FluentAssertions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Domain.Entities;

public class BoardColumnTests
{
    [Fact]
    public void Constructor_ShouldCreateBoardColumn_WhenDataIsValid()
    {
        var boardId = Guid.NewGuid();

        var column = new BoardColumn(boardId, "A fazer", 1);

        column.BoardId.Should().Be(boardId);
        column.Name.Should().Be("A fazer");
        column.Order.Should().Be(1);
        column.Id.Should().NotBeEmpty();
        column.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenBoardIdIsEmpty()
    {
        var action = () => new BoardColumn(Guid.Empty, "A fazer", 1);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("O quadro é obrigatório.*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        var action = () => new BoardColumn(Guid.NewGuid(), "", 1);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("O nome da coluna é obrigatório.*");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenOrderIsNegative()
    {
        var action = () => new BoardColumn(Guid.NewGuid(), "A fazer", -1);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("A ordem da coluna não pode ser negativa.*");
    }

    [Fact]
    public void Rename_ShouldUpdateNameAndUpdatedAt_WhenNameIsValid()
    {
        var column = new BoardColumn(Guid.NewGuid(), "Nome antigo", 1);

        column.Rename("Nome novo");

        column.Name.Should().Be("Nome novo");
        column.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void ChangeOrder_ShouldUpdateOrderAndUpdatedAt_WhenOrderIsValid()
    {
        var column = new BoardColumn(Guid.NewGuid(), "A fazer", 1);

        column.ChangeOrder(2);

        column.Order.Should().Be(2);
        column.UpdatedAt.Should().NotBeNull();
    }
}