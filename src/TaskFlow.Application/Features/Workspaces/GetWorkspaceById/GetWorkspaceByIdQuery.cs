using MediatR;

namespace TaskFlow.Application.Features.Workspaces.GetWorkspaceById;

public record GetWorkspaceByIdQuery(Guid Id) : IRequest<WorkspaceDetailsDto?>;