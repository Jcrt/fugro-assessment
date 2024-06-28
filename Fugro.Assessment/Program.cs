using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Fugro.Assessment.Math.Extensions;
using Fugro.Assessment.Math.Services;
using Fugro.Assessment.Math.Dtos;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddCsvPointsSource();
    });

var app = builder.Build();
app.Start();

var arbitraryPoint = new Point(100, 200, 0);

var mathService = app.Services.GetRequiredService<IMathService>();
var points = await mathService.GetPoints();
var segments = mathService.GetSegments(points);
Console.WriteLine(segments);