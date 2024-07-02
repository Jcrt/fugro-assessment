using Fugro.Assessment.Geometry.Dtos;

namespace Fugro.Assessment.Geometry.Utilities;

internal sealed class GeometryUtility : IGeometryUtility
{
    public Point? CalculateIntersectionPointIfExists(Point A, Point B, Point arbitraryPoint)
    {
        ArgumentNullException.ThrowIfNull(A, nameof(A));
        ArgumentNullException.ThrowIfNull(B, nameof(B));
        ArgumentNullException.ThrowIfNull(arbitraryPoint, nameof(arbitraryPoint));

        double dx = B.X - A.X;
        double dy = B.Y - A.Y;

        double t = ((arbitraryPoint.X - A.X) * dx + (arbitraryPoint.Y - A.Y) * dy) / (dx * dx + dy * dy);

        if (t >= 0 && t <= 1)
        {
            double x = A.X + t * dx;
            double y = A.Y + t * dy;

            return new(x, y);
        }

        return null;
    }

    public double CalculatePerpendicularDistance(double termA, double termB, double termC, Point arbitraryPoint)
    {
        ArgumentNullException.ThrowIfNull(arbitraryPoint, nameof(arbitraryPoint));

        double numerator = Math.Round(Math.Abs((termA * arbitraryPoint.X) + (termB * arbitraryPoint.Y) + termC), 6);
        double denominator = Math.Round(Math.Sqrt((termA * termA) + (termB * termB)), 6);
        return Math.Round(numerator / denominator, 6);
    }

    public (double termA, double termB, double termC) CalculateLineEquation(Point A, Point B)
    {
        ArgumentNullException.ThrowIfNull(A, nameof(A));
        ArgumentNullException.ThrowIfNull(B, nameof(B));

        if (B.X == A.X)
            return (1, 0, -A.X);

        double m = (B.Y - A.Y) / (B.X - A.X);

        double b = A.Y - m * A.X;

        double termA = Math.Round(-m, 6);
        double termB = 1;
        double termC = Math.Round(-b, 6);

        return (termA, termB, termC);
    }

    public double CalcSegmentSize(Point A, Point B)
    {
        ArgumentNullException.ThrowIfNull(A, nameof(A));
        ArgumentNullException.ThrowIfNull(B, nameof(B));

        var deltaX = Math.Pow(B.X - A.X, 2);
        var deltaY = Math.Pow(B.Y - A.Y, 2);
        return Math.Round(Math.Sqrt(deltaX + deltaY), 6);
    }

    public List<Segment> GetSegments(List<Point> points)
    {
        if (points == null || points is { Count: < 2 })
            throw new ArgumentException($"Minimum number of points: 2. Given number of points {points?.Count ?? 0}");

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
