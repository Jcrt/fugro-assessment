using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Geometry.Services;
using Fugro.Assessment.Geometry.Sources;

namespace Fugro.Assessment.Geometry.Extensions;

internal sealed class MathService(IPointsSource pointSource) : IMathService
{
    private readonly IPointsSource _pointSource = pointSource;

    public double CalcHypotenuseSize(Segment adjascentCathetus, Segment oppositeCathetus)
    {
        var adjascentCatethusSize = CalcSegmentSize(adjascentCathetus);
        var oppositeCatethusSize = CalcSegmentSize(oppositeCathetus);

        var hypotenuseSize = Math.Sqrt(Math.Pow(adjascentCatethusSize, 2) + Math.Pow(oppositeCatethusSize, 2));

        return hypotenuseSize;
    }

    public double CalcSegmentSize(Segment segment) => CalcSegmentSize(segment.A, segment.B);
   
    public double CalcSegmentSize(Point A, Point B)
    {
        var deltaX = Math.Pow(B.X - A.X, 2);
        var deltaY = Math.Pow(B.Y - A.Y, 2);
        return Math.Sqrt(deltaX + deltaY);
    }

    public Task<List<Point>> GetPoints() => _pointSource.GetPoints();

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
