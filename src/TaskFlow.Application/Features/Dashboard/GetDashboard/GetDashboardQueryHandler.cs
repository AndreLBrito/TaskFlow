using MediatR;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.Dashboard.GetDashboard;

public class GetDashboardQueryHandler
    : IRequestHandler<GetDashboardQuery, DashboardDto>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly ITaskItemRepository _taskItemRepository;

    public GetDashboardQueryHandler(
        IWorkspaceRepository workspaceRepository,
        IBoardRepository boardRepository,
        ITaskItemRepository taskItemRepository)
    {
        _workspaceRepository = workspaceRepository;
        _boardRepository = boardRepository;
        _taskItemRepository = taskItemRepository;
    }

    public async Task<DashboardDto> Handle(
        GetDashboardQuery request,
        CancellationToken cancellationToken)
    {
        return new DashboardDto
        {
            WorkspacesCount = await _workspaceRepository.CountAsync(cancellationToken),
            BoardsCount = await _boardRepository.CountAsync(cancellationToken),
            TasksCount = await _taskItemRepository.CountAsync(cancellationToken),
            CompletedTasksCount = await _taskItemRepository.CountCompletedAsync(cancellationToken)
        };
    }
}