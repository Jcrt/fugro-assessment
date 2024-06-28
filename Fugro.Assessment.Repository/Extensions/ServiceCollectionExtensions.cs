using Fugro.Assessment.Geometry.Sources;
using Fugro.Assessment.Repository.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fugro.Assessment.Repository.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositoryDependencies(this IServiceCollection services)
    {
        services.TryAddSingleton<IPointsRepository, CsvFilePointsRepository>();
        services.TryAddSingleton<IFileContentProvider, FileContentProvider>();
        return services;
    }
}
