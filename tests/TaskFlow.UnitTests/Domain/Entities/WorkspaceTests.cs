using FluentAssertions;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Domain.Entities;

public class WorkspaceTests
{
    [Fact]
    public void Constructor_ShouldCreateWorkspace_WhenNameIsValid()
    {
        var workspace = new Workspace("Meu workspace", "Descrição do workspace");

        workspace.Name.Should().Be("Meu workspace");
        workspace.Description.Should().Be("Descrição do workspace");
        workspace.Id.Should().NotBeEmpty();
        workspace.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        var action = () => new Workspace("");

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("O nome do workspace é obrigatório.*");
    }

    [Fact]
    public void Rename_ShouldUpdateNameAndUpdatedAt_WhenNameIsValid()
    {
        var workspace = new Workspace("Nome antigo");

        workspace.Rename("Nome novo");

        workspace.Name.Should().Be("Nome novo");
        workspace.UpdatedAt.Should().NotBeNull();
    }
}