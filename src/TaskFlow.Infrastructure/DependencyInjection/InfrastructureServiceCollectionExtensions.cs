using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Interfaces;
using TaskFlow.Infrastructure.Repositories;

namespace TaskFlow.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<IBoardColumnRepository, BoardColumnRepository>();
        services.AddScoped<ITaskItemRepository, TaskItemRepository>();

        return services;
    }
}