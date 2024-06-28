using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Geometry.Extensions;
using Fugro.Assessment.Geometry.Utilities;
using Fugro.Assessment.Geometry.Sources;
using Fugro.Assessment.Routes.Dtos;
using Fugro.Assessment.Routes.Extensions;
using Fugro.Assessment.Routes.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(configuration =>
    {
        configuration.SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, true)
            .Build();
    })
    .ConfigureServices((services) =>
    {
        services.AddGeometryDependencies();
        services.AddRouteDependencies();
    });

var app = builder.Build();
app.Start();

try
{
    var arbitraryPoint = GetPointFromInterface();

    var mathUtility = app.Services.GetRequiredService<IGeometryUtility>();
    var routeService = app.Services.GetRequiredService<IRouteService>();
    var repository = app.Services.GetRequiredService<IPointsRepository>();

    var points = await repository.GetPoints();

    var geometryPoints = points.Select((point, index) => new Point(point.X, point.Y, index)).ToList();
    var segments = mathUtility.GetSegments(geometryPoints);

    var stationSegments = segments.Select(segment => segment.ToStationSegment(StationSegmentType.Segment)).ToList();
    var offsetData = await routeService.GetNearestOffsetData(stationSegments, arbitraryPoint);
    var stationFinalSegments = await routeService.GetStation(stationSegments, offsetData);
    var sumOfStationDistance = await routeService.CalculateStationSize(stationFinalSegments);

    PrintResults(stationFinalSegments, sumOfStationDistance);
} catch(Exception ex)
{
    Console.WriteLine("===================================================");
    Console.WriteLine("=> An error occurred during application execution!");
    Console.WriteLine($"=> {ex.Message}");
}

static Point GetPointFromInterface()
{
    Console.WriteLine("===== Fugro Assessment ==========");
    Console.Write("Please inform a X coord: ");
    var xCoord = Console.ReadLine();
    Console.Write("Please inform a Y coord: ");
    var yCoord = Console.ReadLine();

    if (!double.TryParse(xCoord, out double doubleXCoord))
        throw new InvalidCastException($"Invalid X coord: {xCoord}");

    if (!double.TryParse(yCoord, out double doubleYCoord))
        throw new InvalidCastException($"Invalid Y coord: {yCoord}");

    return new Point(doubleXCoord, doubleYCoord, 0);
}

static void PrintResults(Queue<StationSegment> station, double totalStationDistance)
{
    Console.WriteLine(Environment.NewLine);
    Console.WriteLine("===== Results ===================");
    Console.WriteLine($"The total station value is: {totalStationDistance:F6}");
    Console.WriteLine("The calculated station segments was: ");

    do
    {
        var stationSegment = station.Dequeue();
        Console.WriteLine($"Segment Name: {stationSegment.GetSegmentionName()}, Segment type: {Enum.GetName(stationSegment.Type)}, Segment size: {stationSegment.Size:F6}");
    } while (station.Count > 0);

}