using MediatR;

namespace TaskFlow.Application.Features.Workspaces.GetWorkspaces;

public record GetWorkspacesQuery : IRequest<IReadOnlyList<WorkspaceListItemDto>>;