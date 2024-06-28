using Fugro.Assessment.Repository.Dtos;

namespace Fugro.Assessment.Repository;

public interface IPointsRepository
{
    public Task<List<Point>> GetPoints(CancellationToken cancellationToken = default);
}
