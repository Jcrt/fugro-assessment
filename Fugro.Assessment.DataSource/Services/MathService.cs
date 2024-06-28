using Fugro.Assessment.Math.Dtos;
using Fugro.Assessment.Math.Services;

namespace Fugro.Assessment.Math.Extensions;

internal sealed class MathService(IPointsSource pointSource) : IMathService
{
    private readonly IPointsSource _pointSource = pointSource;

    public double CalcHypotenuseSize(Segment adjascentCathetus, Segment oppositeCathetus)
    {
        throw new NotImplementedException();
    }

    public double CalcSegmentSize(Segment segment)
    {
        throw new NotImplementedException();
    }

    public Task<List<Point>> GetPoints() => _pointSource.GetPoints();

    public List<Segment> GetSegments(List<Point> points)
    {
        if(points == null || points is { Count: 0 })
            throw new ArgumentException($"Minimum number of points: 2. Given number of points {points?.Count ?? 0 }");

        var segments = new List<Segment>();
        var index = 1;

        Point? point1;
        Point? point2;

        while (points.Count > index)
        {
            point1 = points.ElementAt(index - 1);
            point2 = points.ElementAt(index);
            segments.Add(new(point1, point2, segments.Count + 1));
            index++;
        }

        return segments;
    }
}
