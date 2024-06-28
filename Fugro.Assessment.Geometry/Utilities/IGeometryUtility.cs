using Fugro.Assessment.Geometry.Dtos;

namespace Fugro.Assessment.Geometry.Utilities;

public interface IGeometryUtility
{
    public List<Segment> GetSegments(List<Point> points);
    public double CalcSegmentSize(Point A, Point B);
    public Point CalculateIntersectionPoint(Point A, Point B, Point arbitraryPoint);
    public double CalculatePerpendicularDistance(double termA, double termB, double termC, Point arbitraryPoint);
    public (double termA, double termB, double termC) CalculateLineEquation(Point A, Point B);
}
