namespace Fugro.Assessment.Geometry.Dtos;

public record Segment(Point A, Point B, int Order, double Size)
{
    public int Order { get; init; } = Order;
    public Point A { get; init; } = A;
    public Point B { get; init; } = B;
    public double Size { get; init; } = Size;
}
