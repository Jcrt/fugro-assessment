using Fugro.Assessment.Geometry.Utilities;
using Fugro.Assessment.Repository;
using Fugro.Assessment.Repository.Dtos;
using Fugro.Assessment.Routes.Extensions;
using Fugro.Assessment.Routes.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fugro.Assessment.Routes.UnitTests.Services;

public class RouteServiceTests
{
    private readonly IRouteService _routeService;
    private readonly Mock<ILogger<RouteService>> _loggerMock = new();
    private readonly Mock<IPointsRepository> _pointsRepositoryMock = new();
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    public RouteServiceTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddRouteDependencies();
        serviceCollection.AddSingleton(_loggerMock.Object);
        serviceCollection.AddSingleton(_pointsRepositoryMock.Object);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _routeService = serviceProvider.GetRequiredService<IRouteService>();

        _pointsRepositoryMock.Setup(p => p.GetPoints(_cancellationToken))
            .ReturnsAsync(PointList);
    }

    [Theory]
    [InlineData(0, 1, 1, true, 2)]
    [InlineData(0, 2, 2, true, 3)]
    [InlineData(1, 5, 6, true, 6)]
    [InlineData(10, 32, 0, false, 0)]
    public async Task CalculateAsync_ShouldFindCorrectResults(double x, double y, double distance, bool isExists, int numberOfSegments)
    {
        var result = await _routeService.CalculateAsync(new(x, y));
        Assert.NotNull(result);
        Assert.Equal(isExists, result.IsExists);
        Assert.Equal(distance, result.TotalDistance);
        Assert.Equal(numberOfSegments, result.Segments.Count);
    }

    private static List<Point> PointList => new()
    {
        new() { X = 0, Y = 0 },
        new() { X = 0, Y = 1 },
        new() { X = 0, Y = 2 },
        new() { X = 0, Y = 3 },
        new() { X = 0, Y = 4 },
        new() { X = 0, Y = 5 },
    };
}
