using Fugro.Assessment.Geometry.Dtos;
using Fugro.Assessment.Routes.Models;

namespace Fugro.Assessment.Routes.Services;

public interface IRouteService
{
    Task<Result> CalculateAsync(Point arbitraryPoint);
}
