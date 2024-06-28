using Fugro.Assessment.Math.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fugro.Assessment.Math.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCsvPointsSource(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPointsSource, CsvFilePointsSource>();
        serviceCollection.AddSingleton<IMathService, MathService>();
        return serviceCollection;
    }
}
