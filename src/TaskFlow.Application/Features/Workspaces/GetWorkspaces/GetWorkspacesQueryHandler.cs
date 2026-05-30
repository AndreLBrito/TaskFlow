using MediatR;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.Workspaces.GetWorkspaces;

public class GetWorkspacesQueryHandler
    : IRequestHandler<GetWorkspacesQuery, IReadOnlyList<WorkspaceListItemDto>>
{
    private readonly IWorkspaceRepository _workspaceRepository;

    public GetWorkspacesQueryHandler(IWorkspaceRepository workspaceRepository)
    {
        _workspaceRepository = workspaceRepository;
    }

    public async Task<IReadOnlyList<WorkspaceListItemDto>> Handle(
        GetWorkspacesQuery request,
        CancellationToken cancellationToken)
    {
        var workspaces = await _workspaceRepository.GetAllAsync(cancellationToken);

        return workspaces
            .Select(workspace => new WorkspaceListItemDto
            {
                Id = workspace.Id,
                Name = workspace.Name,
                Description = workspace.Description,
                CreatedAt = workspace.CreatedAt
            })
            .ToList();
    }
}