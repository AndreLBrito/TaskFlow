using MediatR;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.Workspaces.GetWorkspaceById;

public class GetWorkspaceByIdQueryHandler : IRequestHandler<GetWorkspaceByIdQuery, WorkspaceDetailsDto?>
{
    private readonly IWorkspaceRepository _workspaceRepository;

    public GetWorkspaceByIdQueryHandler(IWorkspaceRepository workspaceRepository)
    {
        _workspaceRepository = workspaceRepository;
    }

    public async Task<WorkspaceDetailsDto?> Handle(
        GetWorkspaceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (workspace is null)
        {
            return null;
        }

        return new WorkspaceDetailsDto
        {
            Id = workspace.Id,
            Name = workspace.Name,
            Description = workspace.Description,
            CreatedAt = workspace.CreatedAt
        };
    }
}