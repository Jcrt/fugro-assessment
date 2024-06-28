using Fugro.Assessment.Geometry.Dtos;

namespace Fugro.Assessment.Geometry.Services;

public interface IMathService
{
    public Task<List<Point>> GetPoints();
    public List<Segment> GetSegments(List<Point> points);
    public double CalcSegmentSize(Segment segment);
    public double CalcSegmentSize(Point A, Point B);
    public double CalcHypotenuseSize(Segment adjascentCathetus, Segment oppositeCathetus);
}
