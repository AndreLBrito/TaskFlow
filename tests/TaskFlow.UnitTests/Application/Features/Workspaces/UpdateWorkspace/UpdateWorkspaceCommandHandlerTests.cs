using FluentAssertions;
using Moq;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Features.Workspaces.UpdateWorkspace;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Application.Features.Workspaces.UpdateWorkspace;

public class UpdateWorkspaceCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateWorkspace_WhenWorkspaceExists()
    {
        var workspace = new Workspace(
            "Nome antigo",
            "Descrição antiga");

        var repositoryMock = new Mock<IWorkspaceRepository>();

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(
                workspace.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(workspace);

        var handler = new UpdateWorkspaceCommandHandler(
            repositoryMock.Object);

        var command = new UpdateWorkspaceCommand(
            workspace.Id,
            "Nome novo",
            "Descrição nova");

        await handler.Handle(
            command,
            CancellationToken.None);

        workspace.Name.Should().Be("Nome novo");
        workspace.Description.Should().Be("Descrição nova");

        repositoryMock.Verify(
            repository => repository.UpdateAsync(
                workspace,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenWorkspaceDoesNotExist()
    {
        var repositoryMock = new Mock<IWorkspaceRepository>();

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Workspace?)null);

        var handler = new UpdateWorkspaceCommandHandler(
            repositoryMock.Object);

        var action = () => handler.Handle(
            new UpdateWorkspaceCommand(
                Guid.NewGuid(),
                "Nome",
                "Descrição"),
            CancellationToken.None);

        await action
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}