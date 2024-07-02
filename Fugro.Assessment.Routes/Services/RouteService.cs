using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Geometry.Utilities;
using Fugro.Assessment.Repository;
using Fugro.Assessment.Routes.Extensions;
using Fugro.Assessment.Routes.Models;
using Microsoft.Extensions.Logging;

namespace Fugro.Assessment.Routes.Services;

internal sealed class RouteService : IRouteService
{
    private readonly IGeometryUtility _mathUtility;
    private readonly IPointsRepository _pointsRepository;
    private readonly ILogger<RouteService> _routeServiceLogger;

    public RouteService(IGeometryUtility mathUtility, IPointsRepository pointsRepository, ILogger<RouteService> routeServiceLogger)
    {
        _mathUtility = mathUtility;
        _pointsRepository = pointsRepository;
        _routeServiceLogger = routeServiceLogger;
    }

    public async Task<Result> CalculateAsync(Point arbitraryPoint)
    {
        var points = await _pointsRepository.GetPoints();

        var geometryPoints = points.Select((point, index) => new Point(point.X, point.Y, index)).ToList();
        var segments = _mathUtility.GetSegments(geometryPoints);

        var stationSegments = segments.Select(segment => segment.ToStationSegment(StationSegmentType.Segment)).ToList();
        var offsetData = await GetNearestOffsetData(stationSegments, arbitraryPoint);

        if (offsetData != null)
        {
            var stationFinalSegments = await GetStation(stationSegments, offsetData);
            var sumOfStationDistance = await CalculateStationSize(stationFinalSegments);

            return new Result()
            {
                IsExists = true,
                OffsetData = offsetData,
                Segments = stationFinalSegments,
                TotalDistance = Math.Round(sumOfStationDistance, 6)
            };
        }

        return new();
    }

    private async Task<OffsetData?> GetNearestOffsetData(List<StationSegment> segments, Point arbitraryPoint, CancellationToken cancellationToken = default)
    {
        try
        {
            return await Task.FromResult(GetNearestOffsetData(segments, arbitraryPoint, 0));
        }
        catch (Exception ex)
        {
            _routeServiceLogger.LogError(ex, message: $"An error occurred in {nameof(GetNearestOffsetData)}.");
            throw;
        }
    }

    private async Task<double> CalculateStationSize(Queue<StationSegment> segments, CancellationToken cancellationToken = default)
    {
        try
        {
            var segmentSizes = segments.Select(segment => _mathUtility.CalcSegmentSize(segment.A, segment.B));
            var segmentsSum = Math.Round(segmentSizes.Sum(), 6);
            return await Task.FromResult(segmentsSum);
        }
        catch (Exception ex)
        {
            _routeServiceLogger.LogError(ex, message: $"An error occurred in {nameof(CalculateStationSize)}.");
            throw;
        }
    }

    private async Task<Queue<StationSegment>> GetStation(List<StationSegment> segments, OffsetData offsetData, CancellationToken cancellationToken = default)
    {
        int index = 0;
        var routeQueue = new Queue<StationSegment>();

        try
        {
            if (offsetData is null)
                return await Task.FromResult(new Queue<StationSegment>([]));

            while (segments[index].Order < offsetData.Segment.Order)
                routeQueue.Enqueue(segments[index++]);

            var partialSegmentIndex = Math.Max(0, index);
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

            return await Task.FromResult(routeQueue);
        }
        catch (Exception ex)
        {
            _routeServiceLogger.LogError(ex, $"An error occurred in {nameof(GetStation)}");
            throw;
        }
    }

    private OffsetData? GetNearestOffsetData(List<StationSegment> segments, Point arbitraryPoint, int index)
    {
        var segment = segments[index];

        Point nearestPoint;
        OffsetData? offsetData = null;
        double currentDistance = -1;

        var (A, B, C) = _mathUtility.CalculateLineEquation(segment.A, segment.B);
        var intersectionPoint = _mathUtility.CalculateIntersectionPointIfExists(segment.A, segment.B, arbitraryPoint);

        if (intersectionPoint != null)
        {
            nearestPoint = new Point(intersectionPoint.X, intersectionPoint.Y);
            currentDistance = Math.Round(_mathUtility.CalculatePerpendicularDistance(A, B, C, arbitraryPoint));

            offsetData = new OffsetData(currentDistance, segment, nearestPoint, arbitraryPoint);
        }

        var hasMoreItems = ++index < segments.Count;

        if (hasMoreItems)
        {
            var nextOffset = GetNearestOffsetData(segments, arbitraryPoint, index);

            if (nextOffset != null && (currentDistance < 0 || nextOffset?.Distance < currentDistance))
                return nextOffset;
        }

        return offsetData;
    }
}
