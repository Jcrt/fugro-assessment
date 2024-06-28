﻿using Fugro.Assessment.Geometry.Dtos;

namespace Fugro.Assessment.Routes.Dtos;

public record StationSegment : Segment
{
    public StationSegmentType Type { get; init; }

    public StationSegment(Point A, Point B, int Order, double Size, StationSegmentType type) : base(A, B, Order, Size)
    {
        Type = type;
    }

    public string GetSegmentionName() =>
        Type != StationSegmentType.Offset
            ? $"{GetSegmentPrefix()}{Order}"
            : GetSegmentPrefix();

    private string GetSegmentPrefix() => 
        Type != StationSegmentType.Offset ? "S" : "O";
}