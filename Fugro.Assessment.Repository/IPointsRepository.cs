using Fugro.Assessment.Repository.Dtos;

namespace Fugro.Assessment.Geometry.Sources;

public interface IPointsRepository
{
    public Task<List<Point>> GetPoints(CancellationToken cancellationToken = default);
}
