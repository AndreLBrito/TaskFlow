using FluentAssertions;
using Moq;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Features.Workspaces.DeleteWorkspace;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Application.Features.Workspaces.DeleteWorkspace;

public class DeleteWorkspaceCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteWorkspace_WhenWorkspaceExists()
    {
        var workspace = new Workspace(
            "Workspace teste",
            "Descrição");

        var repositoryMock = new Mock<IWorkspaceRepository>();

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(
                workspace.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(workspace);

        var handler = new DeleteWorkspaceCommandHandler(
            repositoryMock.Object);

        await handler.Handle(
            new DeleteWorkspaceCommand(workspace.Id),
            CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.DeleteAsync(
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

        var handler = new DeleteWorkspaceCommandHandler(
            repositoryMock.Object);

        var action = () => handler.Handle(
            new DeleteWorkspaceCommand(Guid.NewGuid()),
            CancellationToken.None);

        await action
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}