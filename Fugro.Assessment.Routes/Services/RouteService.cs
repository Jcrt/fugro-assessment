using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Geometry.Utilities;
using Fugro.Assessment.Routes.Models;
using Microsoft.Extensions.Logging;

namespace Fugro.Assessment.Routes.Services;

internal sealed class RouteService : IRouteService
{
    private readonly IGeometryUtility _mathUtility;
    private readonly ILogger<RouteService> _routeServiceLogger;

    public RouteService(IGeometryUtility mathUtility, ILogger<RouteService> routeServiceLogger)
    {
        _mathUtility = mathUtility;
        _routeServiceLogger = routeServiceLogger;
    }

    public Task<OffsetData> GetNearestOffsetData(List<StationSegment> segments, Point arbitraryPoint, CancellationToken cancellationToken = default) 
    {
        try
        {
            return Task.FromResult(GetNearestOffsetData(segments, arbitraryPoint, 0));
        }
        catch (Exception ex)
        {
            _routeServiceLogger.LogError(ex, message: $"An error occurred in {nameof(GetNearestOffsetData)}.");
            throw;
        }
    }

    public Task<double> CalculateStationSize(Queue<StationSegment> segments, CancellationToken cancellationToken = default)
    {
        try
        {
            var segmentSizes = segments.Select(segment => _mathUtility.CalcSegmentSize(segment.A, segment.B));
            var segmentsSum = Math.Round(segmentSizes.Sum(), 6);
            return Task.FromResult(segmentsSum);
        }
        catch (Exception ex)
        {
            _routeServiceLogger.LogError(ex, message: $"An error occurred in {nameof(CalculateStationSize)}.");
            throw;
        }
    }

    public Task<Queue<StationSegment>> GetStation(List<StationSegment> segments, OffsetData offsetData, CancellationToken cancellationToken = default)
    {
        int index = 0;
        var routeQueue = new Queue<StationSegment>();

        try
        {
            while (segments[index].Order < offsetData.Segment.Order)
                routeQueue.Enqueue(segments[index++]);

            var partialSegmentIndex = Math.Max(0, index - 1);
            var partialLastSegment = new StationSegment(
                segments[partialSegmentIndex].A,
                offsetData.PointInsideSegment,
                ++index,
                _mathUtility.CalcSegmentSize(segments[partialSegmentIndex].A, offsetData.PointInsideSegment),
                StationSegmentType.PartialSegment
            );

            routeQueue.Enqueue(partialLastSegment);

            var offsetSegment = new StationSegment(
                offsetData.PointInsideSegment,
                offsetData.ArbitraryPoint,
                ++index,
                _mathUtility.CalcSegmentSize(offsetData.PointInsideSegment, offsetData.ArbitraryPoint),
                StationSegmentType.Offset
            );

            routeQueue.Enqueue(offsetSegment);

            return Task.FromResult(routeQueue);
        }
        catch (Exception ex)
        {
            _routeServiceLogger.LogError(ex, $"An error occurred in {nameof(GetStation)}");
            throw;
        }
    }

    private OffsetData GetNearestOffsetData(List<StationSegment> segments, Point arbitraryPoint, int index)
    {
        var segment = segments[index];

        Point nearestPoint;
        var (A, B, C) = _mathUtility.CalculateLineEquation(segment.A, segment.B);
        var intersectionPoint = _mathUtility.CalculateIntersectionPoint(segment.A, segment.B, arbitraryPoint);

        nearestPoint = new Point(intersectionPoint.X, intersectionPoint.Y);
        var currentDistance = Math.Round(_mathUtility.CalculatePerpendicularDistance(A, B, C, arbitraryPoint));

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
