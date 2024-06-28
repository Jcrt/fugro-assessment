using Fugro.Assessment.Routes.Services;
using Fugro.Assessment.Geometry.Extensions;
using Fugro.Assessment.Repository.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fugro.Assessment.Routes.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRouteDependencies(this IServiceCollection services) {
        services.AddGeometryDependencies();
        services.AddRepositoryDependencies();
        services.TryAddSingleton<IRouteService, RouteService>();
        return services;
    }
}
