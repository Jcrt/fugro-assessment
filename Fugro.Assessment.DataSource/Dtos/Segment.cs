namespace Fugro.Assessment.Math.Dtos;

public record Segment(Point A, Point B, int Order)
{
    public int Order { get; init; } = Order;
    public Point A { get; init; } = A;
    public Point B { get; init; } = B;
}
