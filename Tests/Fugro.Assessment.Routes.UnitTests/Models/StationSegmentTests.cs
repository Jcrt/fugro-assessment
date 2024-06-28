using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Routes.Dtos;

namespace Fugro.Assessment.Routes.UnitTests.Models;

public class StationSegmentTests
{
    [Fact]
    public void GetSegmentionName_ShouldReturnOffsetPrefix()
    {
        var stationSegment = new StationSegment(new Point(1, 2), new Point(3, 4), 1, 0, StationSegmentType.Offset);
        var name = stationSegment.GetSegmentionName();

        Assert.Equal("O", name);
    }

    [Fact]
    public void GetSegmentionName_ShouldReturnStationPrefixAndNumber()
    {
        var stationSegment = new StationSegment(new Point(1, 2), new Point(3, 4), 1, 0, StationSegmentType.Segment);
        var name = stationSegment.GetSegmentionName();

        Assert.Equal("S1", name);
    }
}
