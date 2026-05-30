using MediatR;

namespace TaskFlow.Application.Features.Workspaces.UpdateWorkspace;

public record UpdateWorkspaceCommand(
    Guid Id,
    string? Name,
    string? Description
) : IRequest;