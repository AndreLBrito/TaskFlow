using MediatR;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.Workspaces.DeleteWorkspace;

public class DeleteWorkspaceCommandHandler
    : IRequestHandler<DeleteWorkspaceCommand>
{
    private readonly IWorkspaceRepository _workspaceRepository;

    public DeleteWorkspaceCommandHandler(
        IWorkspaceRepository workspaceRepository)
    {
        _workspaceRepository = workspaceRepository;
    }

    public async Task Handle(
        DeleteWorkspaceCommand request,
        CancellationToken cancellationToken)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (workspace is null)
        {
            throw new NotFoundException("Workspace não encontrado.");
        }

        await _workspaceRepository.DeleteAsync(
            workspace,
            cancellationToken);
    }
}