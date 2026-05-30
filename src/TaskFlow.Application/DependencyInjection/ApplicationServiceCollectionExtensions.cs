using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using TaskFlow.Application.Behaviors;

namespace TaskFlow.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(
                typeof(ApplicationServiceCollectionExtensions).Assembly);
        });

        services.AddValidatorsFromAssembly(
            typeof(ApplicationServiceCollectionExtensions).Assembly);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}