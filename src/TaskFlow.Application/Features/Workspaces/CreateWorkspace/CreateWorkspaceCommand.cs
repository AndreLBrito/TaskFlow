using MediatR;

namespace TaskFlow.Application.Features.Workspaces.CreateWorkspace;

public record CreateWorkspaceCommand(
    string? Name,
    string? Description
) : IRequest<Guid>;