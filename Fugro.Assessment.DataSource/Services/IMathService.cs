using Fugro.Assessment.Math.Dtos;

namespace Fugro.Assessment.Math.Services;

public interface IMathService
{
    public Task<List<Point>> GetPoints();
    public List<Segment> GetSegments(List<Point> points);
    public double CalcSegmentSize(Segment segment);
    public double CalcHypotenuseSize(Segment adjascentCathetus, Segment oppositeCathetus);
}
