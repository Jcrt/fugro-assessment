using Fugro.Assessment.Geometry.Utilities;
using Fugro.Assessment.Routes.Extensions;
using Fugro.Assessment.Routes.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fugro.Assessment.Routes.UnitTests.Extensions;

public class ServiceCollectionExtensionsTests
{
    private readonly Mock<IConfiguration> _configuration = new();
    private readonly Mock<ILogger<RouteService>> _logger = new();

    [Fact]
    public void AddRouteDependencies_ShouldAddRightServices()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddRouteDependencies();
        serviceCollection.AddSingleton(_configuration.Object);
        serviceCollection.AddSingleton(_logger.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var routeService = serviceProvider.GetRequiredService<IRouteService>();
        var mathService = serviceProvider.GetRequiredService<IGeometryUtility>();

        Assert.NotNull(routeService);
        Assert.NotNull(mathService);
    }
}
