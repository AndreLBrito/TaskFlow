namespace TaskFlow.Application.Features.Dashboard.GetDashboard;

public class DashboardDto
{
    public int WorkspacesCount { get; set; }

    public int BoardsCount { get; set; }

    public int TasksCount { get; set; }

    public int CompletedTasksCount { get; set; }
}