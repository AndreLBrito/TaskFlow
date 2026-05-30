using FluentAssertions;
using TaskFlow.Application.Features.Workspaces.CreateWorkspace;

namespace TaskFlow.UnitTests.Application.Features.Workspaces.CreateWorkspace;

public class CreateWorkspaceCommandValidatorTests
{
    private readonly CreateWorkspaceCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var command = new CreateWorkspaceCommand(
            "Workspace teste",
            "Descrição");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldFail_WhenNameIsEmpty()
    {
        var command = new CreateWorkspaceCommand(
            string.Empty,
            "Descrição");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();

        result.Errors
            .Select(error => error.ErrorMessage)
            .Should()
            .Contain("O nome do workspace é obrigatório.");
    }

    [Fact]
    public void Validate_ShouldFail_WhenNameExceedsMaximumLength()
    {
        var command = new CreateWorkspaceCommand(
            new string('A', 101),
            "Descrição");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();

        result.Errors
            .Select(error => error.ErrorMessage)
            .Should()
            .Contain("O nome do workspace não pode ter mais de 100 caracteres.");
    }
}