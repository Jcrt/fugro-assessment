using Fugro.Assessment.Geometry.Services;
using Fugro.Assessment.Geometry.Sources;
using Fugro.Assessment.Repository.Providers;
using Fugro.Assessment.Routes.Extensions;
using Fugro.Assessment.Routes.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fugro.Assessment.Routes.UnitTests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddRouteDependencies_ShouldAddRightServices()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddRouteDependencies();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var pointsRepository = serviceProvider.GetRequiredService<IPointsRepository>();
        var fileContentProvider = serviceProvider.GetRequiredService<IFileContentProvider>();
        var routeService = serviceProvider.GetRequiredService<IRouteService>();
        var mathService = serviceProvider.GetRequiredService<IMathUtility>();

        Assert.NotNull(pointsRepository);
        Assert.NotNull(fileContentProvider);
        Assert.NotNull(routeService);
        Assert.NotNull(mathService);
    }
}
