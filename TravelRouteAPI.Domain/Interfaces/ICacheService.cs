using TravelRouteAPI.Domain.Enums;
using TravelRouteAPI.Shared.Models;

namespace TravelRouteAPI.Domain.Interfaces;

public interface ICacheService
{
    Task AddRouteAsync(Route route);

    IAsyncEnumerable<Route> SearchRoutesAsync(string origin, string destination, DateTime originDateTime);
}