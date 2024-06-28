using Fugro.Assessment.Repository.Dtos;
using Fugro.Assessment.Repository.Providers;

namespace Fugro.Assessment.Repository;

internal sealed class CsvFilePointsRepository(IFileContentProvider fileContentProvider) : IPointsRepository
{
    private readonly IFileContentProvider _fileContentProvider = fileContentProvider;

    public Task<List<Point>> GetPoints(CancellationToken cancellationToken = default)
    {
        var points = new List<Point>();

        foreach(var row in _fileContentProvider.ReadNext())
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

    private static double ParseToDouble(string coord)
    {
        if (!double.TryParse(coord, out double doubleCoord))
            throw new InvalidCastException($"The value '{coord}' couldn't be parsed to double. Check your data source");

        return doubleCoord;
    }
}
