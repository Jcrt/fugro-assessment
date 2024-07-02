using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Geometry.Extensions;
using Fugro.Assessment.Repository.Extensions;
using Fugro.Assessment.Routes.Extensions;
using Fugro.Assessment.Routes.Models;
using Fugro.Assessment.Routes.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        services.AddRepositoryDependencies();
        services.AddLogging();
    });

var app = builder.Build();
app.Start();

try
{
    var arbitraryPoint = GetPointFromInterface();

    var routeService = app.Services.GetRequiredService<IRouteService>();
    var result = await routeService.CalculateAsync(arbitraryPoint);

    PrintResults(result);
}
catch (Exception ex)
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

static void PrintResults(Result result)
{
    Console.WriteLine(Environment.NewLine);
    Console.WriteLine("===== Results ===================");

    if (result.IsExists)
    {
        Console.WriteLine($"The total station value is: {result.TotalDistance:F6}");
        Console.WriteLine("The calculated station segments was: ");

        do
        {
            var stationSegment = result.Segments.Dequeue();
            Console.WriteLine($"Segment Name: {stationSegment.GetSegmentionName()}, Segment type: {Enum.GetName(stationSegment.Type)}, Segment size: {stationSegment.Size:F6}");
        } while (result.Segments.Count > 0);

        Console.WriteLine($"{Environment.NewLine}Press ENTER to close application...");
        Console.ReadLine();
    }
    else
    {
        Console.WriteLine($"Doesn't exist any segment that attend the perpendicular requirement 😿");
    }
}