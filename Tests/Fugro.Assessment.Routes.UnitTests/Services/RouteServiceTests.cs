using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Routes.Dtos;
using Fugro.Assessment.Routes.Extensions;
using Fugro.Assessment.Routes.Models;
using Fugro.Assessment.Routes.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fugro.Assessment.Routes.UnitTests.Services;

public class RouteServiceTests
{
    private readonly IRouteService _routeService;
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    public RouteServiceTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddRouteDependencies();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _routeService = serviceProvider.GetRequiredService<IRouteService>();
    }

    [Fact]
    public async Task GetNearestOffsetData_ReturnRightValues()
    {
        var segments = new List<StationSegment>()
        {
            new(PointList[0], PointList[1], 1, 0),
            new(PointList[2], PointList[3], 2, 0),
            new(PointList[4], PointList[5], 3, 0)
        };

        var arbitraryPoint = new Point(10, 30);

        var offsetData = await _routeService.GetNearestOffsetData(segments, arbitraryPoint, _cancellationToken);

        Assert.Equal(0, offsetData.Distance);
        Assert.Equal(1, offsetData.Segment.Order);
        Assert.Equal(arbitraryPoint, offsetData.ArbitraryPoint);
    }

    [Theory]
    [InlineData(10, 30, 0)]
    [InlineData(130, 3230, 1)]
    [InlineData(50, -81, 2)]
    public async Task GetStation_ReturnCorrectStations(double X, double Y, int numberOfStationSegments)
    {
        var segments = new List<StationSegment>()
        {
            new(PointList[0], PointList[1], 1, 0),
            new(PointList[2], PointList[3], 2, 0),
            new(PointList[4], PointList[5], 3, 0)
        };

        var arbitraryPoint = new Point(X, Y);
        var offsetData = new OffsetData(10, segments[numberOfStationSegments], arbitraryPoint, arbitraryPoint);

        var stations = await _routeService.GetStation(segments, offsetData, _cancellationToken);
        Assert.Equal(segments[numberOfStationSegments].Order, stations.First(x => x.Type ==  StationSegmentType.PartialSegment).Order);
        Assert.Contains(stations, x => x.GetSegmentionName() == "O" && x.Type == StationSegmentType.Offset);
        Assert.Contains(stations, x => x.GetSegmentionName().StartsWith('S') && x.Type == StationSegmentType.PartialSegment);

    }

    [Fact]
    public async Task CalculateStationSize_GetCorrectValue()
    {
        var segments = new Queue<StationSegment>();
        segments.Enqueue(new(PointList[0], PointList[1], 1, 0));
        segments.Enqueue(new(PointList[1], PointList[2], 2, 0));
        segments.Enqueue(new(PointList[2], PointList[3], 3, 0));
       
        var sum = await _routeService.CalculateStationSize(segments, _cancellationToken);
        Assert.Equal(3218.693999, sum);
    }

    private static List<Point> PointList => new()
    {
        new Point(10, 30),
        new Point(20, 60),
        new Point(50, 80),
        new Point(130, 3230),
        new Point(-120, 604),
        new Point(50, -80),
    };
}
