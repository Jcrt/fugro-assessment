using Fugro.Assessment.Math.Dtos;

namespace Fugro.Assessment.Math;

public interface IPointsSource
{
    public Task<List<Point>> GetPoints(CancellationToken cancellationToken = default);
}
