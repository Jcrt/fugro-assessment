using Fugro.Assessment.Geometry.Utilities;
using Fugro.Assessment.Repository.Providers;
using Fugro.Assessment.Routes.Extensions;
using Fugro.Assessment.Routes.Services;
using Microsoft.Extensions.DependencyInjection;
using Fugro.Assessment.Repository;

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
        var mathService = serviceProvider.GetRequiredService<IGeometryUtility>();

        Assert.NotNull(pointsRepository);
        Assert.NotNull(fileContentProvider);
        Assert.NotNull(routeService);
        Assert.NotNull(mathService);
    }
}
