using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Geometry.Utilities;

namespace Fugro.Assessment.Geometry.UnitTests.Utilities;

public class GeometryUtilityTests
{
    private readonly GeometryUtility _geometryUtility = new();

    [Fact]
    public void GetSegments_ShouldReturnCorrectSegments()
    {
        var segments = _geometryUtility.GetSegments(PointList);

        Assert.Equal(2, segments.Count);
        Assert.Equal(PointList[0], segments[0].A);
        Assert.Equal(PointList[1], segments[0].B);
        Assert.Equal(PointList[1], segments[1].A);
        Assert.Equal(PointList[2], segments[1].B);
    }

    [Fact]
    public void GetSegments_ShouldThrowExceptionIfNotEnoughPoints()
    {
        Assert.Throws<ArgumentException>(() => _geometryUtility.GetSegments(PointList.Take(1).ToList()));
    }

    [Fact]
    public void CalcSegmentSize_ShouldCalculateCorrectly()
    {
        var segmentSize = _geometryUtility.CalcSegmentSize(PointList[0], PointList[1]);
        Assert.Equal(1, segmentSize);
    }

    [Fact]
    public void CalcSegmentSize_ShouldThrowIfArgumentsAreNull()
    {
        Assert.Throws<ArgumentNullException>(() => _geometryUtility.CalcSegmentSize(PointList[0], PointList.ElementAtOrDefault(10)));
        Assert.Throws<ArgumentNullException>(() => _geometryUtility.CalcSegmentSize(PointList.ElementAtOrDefault(10), PointList[1]));
    }

    [Fact]
    public void CalculateIntersectionPoint_ShoudReturnCorrectPoint()
    {
        var intersectionPoint = _geometryUtility.CalculateIntersectionPointIfExists(PointList[0], PointList[1], PointList[2]);

        Assert.NotNull(intersectionPoint);
        Assert.Equal(0, intersectionPoint.Order);
        Assert.Equal(0, intersectionPoint.X);
        Assert.Equal(1, intersectionPoint.Y);
    }

    [Fact]
    public void CalculateIntersectionPoint_ShouldReturnNull()
    {
        var intersectionPoint = _geometryUtility.CalculateIntersectionPointIfExists(PointList[0], PointList[1], new(5, 10));

        Assert.Null(intersectionPoint);
    }

    [Fact]
    public void CalculateIntersectionPoint_ShouldThrowIfArgumentsAreNull()
    {
        Assert.Throws<ArgumentNullException>(() => _geometryUtility.CalculateIntersectionPointIfExists(null, PointList[1], PointList[2]));
        Assert.Throws<ArgumentNullException>(() => _geometryUtility.CalculateIntersectionPointIfExists(PointList[0], null, PointList[2]));
        Assert.Throws<ArgumentNullException>(() => _geometryUtility.CalculateIntersectionPointIfExists(PointList[0], PointList[1], null));
    }

    [Fact]
    public void CalculateLineEquation_ShouldReturnMembers()
    {
        var lineEquation = _geometryUtility.CalculateLineEquation(PointList[1], PointList[2]);

        Assert.Equal(0, lineEquation.termA);
        Assert.Equal(1, lineEquation.termB);
        Assert.Equal(-1, lineEquation.termC);
    }

    [Fact]
    public void CalculateLineEquation_ShouldThrowIfArgumentsAreNull()
    {
        Assert.Throws<ArgumentNullException>(() => _geometryUtility.CalculateLineEquation(null, PointList[1]));
        Assert.Throws<ArgumentNullException>(() => _geometryUtility.CalculateLineEquation(PointList[0], null));
    }

    [Fact]
    public void CalculatePerpendicularDistance_ShouldCalculateCorrectly()
    {
        var termA = -0;
        var termB = 1;
        var termC = -1;

        var perperndicularDistance = _geometryUtility.CalculatePerpendicularDistance(termA, termB, termC, PointList[2]);
        Assert.Equal(0, perperndicularDistance);
    }

    [Fact]
    public void CalculatePerpendicularDistance_ShouldThrowIfArgumentsAreNull()
    {
        Assert.Throws<ArgumentNullException>(() => _geometryUtility.CalculatePerpendicularDistance(1, 2, 4, null));
    }

    private static List<Point> PointList => new()
    {
        new Point(0, 0),
        new Point(0, 1),
        new Point(1, 1)
    };
}
