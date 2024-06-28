using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Routes.Dtos;
using Fugro.Assessment.Routes.Extensions;

namespace Fugro.Assessment.Routes.UnitTests.Extensions;

public class SegmentExtensionTests
{
    [Fact]
    public void ToStationSegment_ShouldCastFromSegmentToStationSegmentCorrectly()
    {
        var segment = new Segment(new(1, 2), new(3, 4), 1, 10);
        var stationSegment = segment.ToStationSegment();

        Assert.NotNull(stationSegment);
        Assert.Equal(segment.Size, stationSegment.Size);
        Assert.Equal(segment.A, stationSegment.A);
        Assert.Equal(segment.B, stationSegment.B);
        Assert.Equal(segment.Order, stationSegment.Order);
        Assert.Equal(StationSegmentType.Segment, stationSegment.Type);
    }
}
