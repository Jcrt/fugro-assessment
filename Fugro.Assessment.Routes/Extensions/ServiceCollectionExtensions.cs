using Fugro.Assessment.Routes.Services;
using Fugro.Assessment.Geometry.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fugro.Assessment.Routes.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRouteDependencies(this IServiceCollection services) {
        services.AddGeometryDependencies();
        services.TryAddSingleton<IRouteService, RouteService>();
        return services;
    }
}
