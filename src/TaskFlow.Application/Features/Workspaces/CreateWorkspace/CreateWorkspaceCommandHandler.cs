using MediatR;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Features.Workspaces.CreateWorkspace;

public class CreateWorkspaceCommandHandler : IRequestHandler<CreateWorkspaceCommand, Guid>
{
    private readonly IWorkspaceRepository _workspaceRepository;

    public CreateWorkspaceCommandHandler(
        IWorkspaceRepository workspaceRepository)
    {
        _workspaceRepository = workspaceRepository;
    }

    public async Task<Guid> Handle(
        CreateWorkspaceCommand request,
        CancellationToken cancellationToken)
    {
        var workspace = new Workspace(
            request.Name!,
            request.Description);

        await _workspaceRepository.AddAsync(
            workspace,
            cancellationToken);

        return workspace.Id;
    }
}