using FluentAssertions;
using Moq;
using TaskFlow.Application.Features.Workspaces.GetWorkspaces;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.UnitTests.Application.Features.Workspaces.GetWorkspaces;

public class GetWorkspacesQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnAllWorkspaces()
    {
        var workspaces = new List<Workspace>
        {
            new("Workspace A", "Descrição A"),
            new("Workspace B", "Descrição B")
        };

        var repositoryMock = new Mock<IWorkspaceRepository>();

        repositoryMock
            .Setup(repository => repository.GetAllAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(workspaces);

        var handler = new GetWorkspacesQueryHandler(
            repositoryMock.Object);

        var result = await handler.Handle(
            new GetWorkspacesQuery(),
            CancellationToken.None);

        result.Should().HaveCount(2);

        result[0].Name.Should().Be("Workspace A");
        result[1].Name.Should().Be("Workspace B");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoWorkspacesExist()
    {
        var repositoryMock = new Mock<IWorkspaceRepository>();

        repositoryMock
            .Setup(repository => repository.GetAllAsync(
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Workspace>());

        var handler = new GetWorkspacesQueryHandler(
            repositoryMock.Object);

        var result = await handler.Handle(
            new GetWorkspacesQuery(),
            CancellationToken.None);

        result.Should().BeEmpty();
    }
}