using MediatR;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.Workspaces.UpdateWorkspace;

public class UpdateWorkspaceCommandHandler
    : IRequestHandler<UpdateWorkspaceCommand>
{
    private readonly IWorkspaceRepository _workspaceRepository;

    public UpdateWorkspaceCommandHandler(
        IWorkspaceRepository workspaceRepository)
    {
        _workspaceRepository = workspaceRepository;
    }

    public async Task Handle(
        UpdateWorkspaceCommand request,
        CancellationToken cancellationToken)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (workspace is null)
        {
            throw new KeyNotFoundException("Workspace não encontrado.");
        }

        workspace.Rename(request.Name!);
        workspace.UpdateDescription(request.Description);

        await _workspaceRepository.UpdateAsync(
            workspace,
            cancellationToken);
    }
}