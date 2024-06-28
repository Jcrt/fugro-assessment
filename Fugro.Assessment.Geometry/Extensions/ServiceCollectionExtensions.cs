using Fugro.Assessment.Geometry.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fugro.Assessment.Geometry.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGeometryDependencies(this IServiceCollection services)
    {
        services.TryAddSingleton<IMathUtility, MathUtility>();
        return services;
    }
}
