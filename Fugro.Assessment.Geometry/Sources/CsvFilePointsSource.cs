using Fugro.Assessment.Geometry.Dtos;

namespace Fugro.Assessment.Geometry.Sources;

internal sealed class CsvFilePointsSource : IPointsSource
{
    //TODO: Add config provider to inform file path
    const string _fileName = "polyline sample.csv";
    readonly string _filePath;

    public CsvFilePointsSource()
    {
        _filePath = Path.Combine(AppContext.BaseDirectory, "assets", _fileName);

        if (!File.Exists(_filePath))
            throw new FileNotFoundException(nameof(_filePath));
    }

    //TODO: Remember to try to use IAsyncEnumerable to save resources in case of error
    public async Task<List<Point>> GetPoints(CancellationToken cancellationToken = default)
    {
        string row;
        var points = new List<Point>();
        using var fileStream = File.OpenRead(_filePath);
        using var reader = new StreamReader(fileStream);

        while (!string.IsNullOrEmpty(row = await reader.ReadLineAsync(cancellationToken) ?? string.Empty))
        {
            var coords = row.Split(',');
            var x = ParseToDouble(coords[0]);
            var y = ParseToDouble(coords[1]);

            points.Add(new(x, y, points.Count + 1));
        }

        return points;
    }

    private static double ParseToDouble(string coord)
    {
        if (!double.TryParse(coord, out double doubleCoord))
            throw new InvalidCastException($"The value '{coord}' couldn't be parsed to double. Check your data source");

        return doubleCoord;
    }
}
