using Fugro.Assessment.Repository.Dtos;
using Fugro.Assessment.Repository.Providers;
using Microsoft.Extensions.Logging;

namespace Fugro.Assessment.Repository;

internal sealed class CsvFilePointsRepository(IFileContentProvider fileContentProvider, ILogger<CsvFilePointsRepository> logger) : IPointsRepository
{
    private readonly IFileContentProvider _fileContentProvider = fileContentProvider;
    private readonly ILogger<CsvFilePointsRepository> _logger = logger;

    public Task<List<Point>> GetPoints(CancellationToken cancellationToken = default)
    {
        try
        {
            var points = new List<Point>();

            foreach (var row in _fileContentProvider.ReadNext())
            {
                var coords = row.Split(',');
                var x = ParseToDouble(coords[0]);
                var y = ParseToDouble(coords[1]);

                points.Add(new()
                {
                    X = x,
                    Y = y
                });
            }

            return Task.FromResult(points);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred in {nameof(CsvFilePointsRepository)}");
            throw;
        }

    }

    private static double ParseToDouble(string coord)
    {
        if (!double.TryParse(coord, out double doubleCoord))
            throw new InvalidCastException($"The value '{coord}' couldn't be parsed to double. Check your data source");

        return doubleCoord;
    }
}
