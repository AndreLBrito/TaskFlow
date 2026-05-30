using FluentAssertions;
using Moq;
using TaskFlow.Application.Features.Workspaces.GetWorkspaceById;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Application.Features.Workspaces.GetWorkspaceById;

public class GetWorkspaceByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnWorkspace_WhenWorkspaceExists()
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

        var handler = new GetWorkspaceByIdQueryHandler(
            repositoryMock.Object);

        var result = await handler.Handle(
            new GetWorkspaceByIdQuery(workspace.Id),
            CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(workspace.Id);
        result.Name.Should().Be(workspace.Name);
        result.Description.Should().Be(workspace.Description);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenWorkspaceDoesNotExist()
    {
        var repositoryMock = new Mock<IWorkspaceRepository>();

        repositoryMock
            .Setup(repository => repository.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Workspace?)null);

        var handler = new GetWorkspaceByIdQueryHandler(
            repositoryMock.Object);

        var result = await handler.Handle(
            new GetWorkspaceByIdQuery(Guid.NewGuid()),
            CancellationToken.None);

        result.Should().BeNull();
    }
}