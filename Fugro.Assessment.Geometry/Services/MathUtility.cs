using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Geometry.Services;
using Fugro.Assessment.Geometry.Sources;

namespace Fugro.Assessment.Geometry.Extensions;

internal sealed class MathUtility : IMathUtility
{
    public Point CalculateIntersectionPoint(Point A, Point B, Point arbitraryPoint)
    {
        double dx = B.X - A.X;
        double dy = B.Y - A.Y;

        double t = ((arbitraryPoint.X - A.X) * dx + (arbitraryPoint.Y - A.Y) * dy) / (dx * dx + dy * dy);

        double x = A.X + t * dx;
        double y = A.Y + t * dy;

        return new(x, y);
    }

    public double CalculatePerpendicularDistance(double termA, double termB, double termC, Point arbitraryPoint)
    {
        double numerator = Math.Abs((termA * arbitraryPoint.X) + (termB * arbitraryPoint.Y) + termC);
        double denominator = Math.Sqrt((termA * termA) + (termB * termB));
        return numerator / denominator;
    }

    public (double termA, double termB, double termC) CalculateLineEquation(Point A, Point B)
    {
        double m = (B.Y - A.Y) / (B.X - A.X);

        double b = A.Y - m * A.X;

        double termA = -m;
        double termB = 1;
        double termC = -b;

        return (termA, termB, termC);
    }

    public double CalcSegmentSize(Segment segment) => CalcSegmentSize(segment.A, segment.B);
   
    public double CalcSegmentSize(Point A, Point B)
    {
        var deltaX = Math.Pow(B.X - A.X, 2);
        var deltaY = Math.Pow(B.Y - A.Y, 2);
        return Math.Sqrt(deltaX + deltaY);
    }

    public List<Segment> GetSegments(List<Point> points)
    {
        if(points == null || points is { Count: 0 })
            throw new ArgumentException($"Minimum number of points: 2. Given number of points {points?.Count ?? 0 }");

        var segments = new List<Segment>();
        var index = 1;

        Point? pointA;
        Point? pointB;

        while (points.Count > index)
        {
            pointA = points.ElementAt(index - 1);
            pointB = points.ElementAt(index);
            segments.Add(new(pointA, pointB, segments.Count + 1, CalcSegmentSize(pointA, pointB)));
            index++;
        }

        return segments;
    }
}
