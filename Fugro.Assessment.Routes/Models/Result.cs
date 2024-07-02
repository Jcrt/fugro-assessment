namespace Fugro.Assessment.Routes.Models;

public class Result
{
    public bool IsExists { get; init; } = false;
    public OffsetData? OffsetData { get; init; }
    public Queue<StationSegment> Segments { get; init; } = new Queue<StationSegment>();
    public double TotalDistance { get; init; } = 0;
}
