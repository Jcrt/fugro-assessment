using Fugro.Assessment.Geometry.Dtos;

namespace Fugro.Assessment.Routes.Models;

public record OffsetData(
    double Distance,
    StationSegment Segment,
    Point PointInsideSegment,
    Point ArbitraryPoint
)
{
    public double Distance { get; init; } = Distance;
    public StationSegment Segment { get; init; } = Segment;
    public Point PointInsideSegment { get; init; } = PointInsideSegment;
    public Point ArbitraryPoint { get; init; } = ArbitraryPoint;
}
