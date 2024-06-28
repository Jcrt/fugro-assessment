using Fugro.Assessment.Geometry.Services;
using Fugro.Assessment.Geometry.Sources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fugro.Assessment.Geometry.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGeometryDependencies(this IServiceCollection services)
    {
        services.TryAddSingleton<IPointsSource, CsvFilePointsSource>();
        services.TryAddSingleton<IMathService, MathService>();
        return services;
    }
}
