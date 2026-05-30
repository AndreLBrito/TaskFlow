using FluentAssertions;
using Moq;
using TaskFlow.Application.Features.Workspaces.CreateWorkspace;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Application.Features.Workspaces.CreateWorkspace;

public class CreateWorkspaceCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateWorkspace_WhenCommandIsValid()
    {
        var repositoryMock = new Mock<IWorkspaceRepository>();

        var handler = new CreateWorkspaceCommandHandler(
            repositoryMock.Object);

        var command = new CreateWorkspaceCommand(
            "Workspace teste",
            "Descrição");

        var workspaceId = await handler.Handle(
            command,
            CancellationToken.None);

        workspaceId.Should().NotBeEmpty();

        repositoryMock.Verify(
            repository => repository.AddAsync(
                It.IsAny<Workspace>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}