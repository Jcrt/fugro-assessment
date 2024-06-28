using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Geometry.Services;
using Fugro.Assessment.Routes.Dtos;

namespace Fugro.Assessment.Routes.Services;

internal sealed class RouteService(IMathService mathService) : IRouteService
{
    private readonly IMathService _mathService = mathService;

    public Task<double> CalculateStationSize(Queue<StationSegment> segments, CancellationToken cancellationToken = default) => 
        Task.FromResult(Math.Round(segments.Select(segment => _mathService.CalcSegmentSize(segment.A, segment.B)).Sum(), 6));
    
    public Task<OffsetData> GetNearestOffsetData(List<StationSegment> segments, Point arbitraryPoint, CancellationToken cancellationToken = default) =>
        Task.FromResult(GetNearestOffsetData(segments, arbitraryPoint, 0) ?? throw new Exception());

    public Task<Queue<StationSegment>> GetStation(List<StationSegment> segments, OffsetData offsetData, CancellationToken cancellationToken = default)
    {
        int index = 0;
        var routeQueue = new Queue<StationSegment>();

        while (segments[index].Order < offsetData.Segment.Order)
            routeQueue.Enqueue(segments[index++]);

        var partialSegmentIndex = Math.Max(0, index - 1);
        var partialLastSegment = new StationSegment(
            segments[partialSegmentIndex].A, 
            offsetData.PointInsideSegment,
            ++index, 
            _mathService.CalcSegmentSize(segments[partialSegmentIndex].A, offsetData.PointInsideSegment),
            StationSegmentType.PartialSegment
        );

        routeQueue.Enqueue(partialLastSegment);

        var offsetSegment = new StationSegment(
            offsetData.PointInsideSegment, 
            offsetData.ArbitraryPoint, 
            index,
            _mathService.CalcSegmentSize(offsetData.PointInsideSegment, offsetData.ArbitraryPoint),
            StationSegmentType.Offset
        );

        routeQueue.Enqueue(offsetSegment);

        return Task.FromResult(routeQueue);
    }

    private OffsetData GetNearestOffsetData(List<StationSegment> segments, Point arbitraryPoint, int index)
    {
        var segment = segments[index];

        Point nearestPoint;
        var deltaX = segment.B.X - segment.A.X;
        var deltaY = segment.B.Y - segment.A.Y;
        var projectionDeltaX = arbitraryPoint.X - segment.A.X;
        var projectionDeltaY = arbitraryPoint.Y - segment.A.Y;

        var AB_AB = Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2);
        var AP_AB = (projectionDeltaX * deltaX) + (projectionDeltaY * deltaY);

        var t = AP_AB / AB_AB;

        if (t < 0)
        {
            nearestPoint = segment.A;
        }
        else if (t > 1)
        {
            nearestPoint = segment.B;
        }
        else
        {
            nearestPoint = new(segment.A.X + t * deltaX, segment.A.Y + t * deltaY, 0);
        }

        var currentDistance = Math.Round(_mathService.CalcSegmentSize(nearestPoint, arbitraryPoint), 6);
        var offsetData = new OffsetData(currentDistance, segment, nearestPoint, arbitraryPoint);

        var hasMoreItems = ++index < segments.Count;

        if (hasMoreItems)
        {
            var nextOffset = GetNearestOffsetData(segments, arbitraryPoint, index);
            if (nextOffset.Distance < currentDistance)
                return nextOffset;
        }

        return offsetData;
    }
}
