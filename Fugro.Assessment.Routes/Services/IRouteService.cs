using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Routes.Dtos;

namespace Fugro.Assessment.Routes.Services;

public interface IRouteService
{
    Task<OffsetData> GetNearestOffsetData(List<StationSegment> segments, Point arbitraryPoint, CancellationToken cancellationToken = default);

    Task<Queue<StationSegment>> GetStation(List<StationSegment> segments, OffsetData offsetData, CancellationToken cancellationToken = default);

    Task<double> CalculateStationSize(Queue<StationSegment> segments, CancellationToken cancellationToken = default);
}
