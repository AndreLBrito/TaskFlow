using MediatR;

namespace TaskFlow.Application.Features.Workspaces.DeleteWorkspace;

public record DeleteWorkspaceCommand(Guid Id) : IRequest;