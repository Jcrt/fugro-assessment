using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Geometry.Services;
using Fugro.Assessment.Routes.Dtos;

namespace Fugro.Assessment.Routes.Services;

internal sealed class RouteService : IRouteService
{
    private readonly IMathUtility _mathService;

    public RouteService(IMathUtility mathService)
    {
        _mathService = mathService; 
    }

    public Task<double> CalculateStationSize(Queue<StationSegment> segments, CancellationToken cancellationToken = default)
    {
        var segmentSizes = segments.Select(segment => _mathService.CalcSegmentSize(segment.A, segment.B));
        var segmentsSum = Math.Round(segmentSizes.Sum(), 6);
        return Task.FromResult(segmentsSum);

    }

    public Task<OffsetData> GetNearestOffsetData(List<StationSegment> segments, Point arbitraryPoint, CancellationToken cancellationToken = default) =>
        Task.FromResult(GetNearestOffsetData(segments, arbitraryPoint, 0));

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
        var (A, B, C) = _mathService.CalculateLineEquation(segment.A, segment.B);
        var intersectionPoint = _mathService.CalculateIntersectionPoint(segment.A, segment.B, arbitraryPoint);
        
        nearestPoint = new Point(intersectionPoint.X, intersectionPoint.Y);
        var currentDistance = Math.Round(_mathService.CalculatePerpendicularDistance(A, B, C, arbitraryPoint));

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
