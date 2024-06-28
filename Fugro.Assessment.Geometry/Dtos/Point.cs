namespace Fugro.Assessment.Geometry.Dtos;

public record Point(double X, double Y, int Order = 0)
{
    public int Order { get; init; } = Order;
    public double X { get; init; } = X;
    public double Y { get; init; } = Y;
}
