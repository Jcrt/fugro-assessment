using Fugro.Assessment.Geometry.Dtos;

namespace Fugro.Assessment.Routes.Models;

public record StationSegment : Segment
{
    public StationSegmentType Type { get; init; }

    public StationSegment(
        Point A,
        Point B,
        int Order,
        double Size,
        StationSegmentType type = StationSegmentType.Segment
    ) : base(A, B, Order, Size)
    {
        Type = type;
    }

    public string GetSegmentionName() =>
        Type != StationSegmentType.Offset
            ? $"{GetSegmentPrefix()}{Order}"
            : GetSegmentPrefix();

    private string GetSegmentPrefix() =>
        Type != StationSegmentType.Offset ? "S" : "O";
}
