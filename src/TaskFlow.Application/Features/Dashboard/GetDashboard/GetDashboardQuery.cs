using MediatR;

namespace TaskFlow.Application.Features.Dashboard.GetDashboard;

public record GetDashboardQuery
    : IRequest<DashboardDto>;