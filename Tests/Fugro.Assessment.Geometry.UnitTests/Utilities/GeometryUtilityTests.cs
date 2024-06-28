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
        Assert.Equal(31.622777, segmentSize);
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
        var intersectionPoint = _geometryUtility.CalculateIntersectionPoint(PointList[0], PointList[1], PointList[2]);
        
        Assert.NotNull(intersectionPoint);
        Assert.Equal(0, intersectionPoint.Order);
        Assert.Equal(29, intersectionPoint.X);
        Assert.Equal(87, intersectionPoint.Y);
    }

    [Fact]
    public void CalculateIntersectionPoint_ShouldThrowIfArgumentsAreNull()
    {
        Assert.Throws<ArgumentNullException>(() => _geometryUtility.CalculateIntersectionPoint(null, PointList[1], PointList[2]));
        Assert.Throws<ArgumentNullException>(() => _geometryUtility.CalculateIntersectionPoint(PointList[0], null, PointList[2]));
        Assert.Throws<ArgumentNullException>(() => _geometryUtility.CalculateIntersectionPoint(PointList[0], PointList[1], null));
    }

    [Fact]
    public void CalculateLineEquation_ShouldReturnMembers()
    {
        var a = _geometryUtility.CalculateLineEquation(PointList[1], PointList[2]);
        
        Assert.Equal(-0.666667, a.termA);
        Assert.Equal(1, a.termB);
        Assert.Equal(-46.666667, a.termC);
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
        var termA = -0.666667;
        var termB = 1;
        var termC = -46.666667;

        var perperndicularDistance = _geometryUtility.CalculatePerpendicularDistance(termA, termB, termC, PointList[2]);
        Assert.Equal(1.4E-05, perperndicularDistance);
    }

    [Fact]
    public void CalculatePerpendicularDistance_ShouldThrowIfArgumentsAreNull()
    {
        Assert.Throws<ArgumentNullException>(() => _geometryUtility.CalculatePerpendicularDistance(1, 2, 4, null));
    }

    private static List<Point> PointList => new()
    {
        new Point(10, 30),
        new Point(20, 60),
        new Point(50, 80)
    };
}
