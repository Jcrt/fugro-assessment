using Fugro.Assessment.Geometry.Dtos;

namespace Fugro.Assessment.Geometry.Sources;

public interface IPointsSource
{
    public Task<List<Point>> GetPoints(CancellationToken cancellationToken = default);
}
