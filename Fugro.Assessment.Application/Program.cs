using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Fugro.Assessment.Geometry.Extensions;
using Fugro.Assessment.Geometry.Services;
using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Routes.Extensions;
using Fugro.Assessment.Routes.Services;
using Fugro.Assessment.Routes.Dtos;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddGeometryDependencies();
        services.AddRouteDependencies();
    });

var app = builder.Build();
app.Start();

var arbitraryPoint = GetPointFromInterface();

var mathService = app.Services.GetRequiredService<IMathService>();
var routeService = app.Services.GetRequiredService<IRouteService>();
var points = await mathService.GetPoints();
var segments = mathService.GetSegments(points);

var stationSegments = segments.Select(segment => segment.ToStationSegment(StationSegmentType.Segment)).ToList();
var offsetData = await routeService.GetNearestOffsetData(stationSegments, arbitraryPoint);
var stationFinalSegments = await routeService.GetStation(stationSegments, offsetData);
var sumOfStationDistance = await routeService.CalculateStationSize(stationFinalSegments);

PrintStationPath(stationFinalSegments, sumOfStationDistance);

Console.ReadKey();

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

static void PrintStationPath(Queue<StationSegment> station, double totalStationDistance)
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