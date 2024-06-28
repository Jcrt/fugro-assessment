using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Routes.Models;

namespace Fugro.Assessment.Routes.Extensions;

public static class SegmentExtension
{
    public static StationSegment ToStationSegment(this Segment segment, StationSegmentType type = StationSegmentType.Segment) =>
        new(segment.A, segment.B, segment.Order, segment.Size, type);
    
}
