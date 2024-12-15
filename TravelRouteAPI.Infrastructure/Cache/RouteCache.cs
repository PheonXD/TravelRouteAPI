using TravelRouteAPI.Shared.Models;

namespace TravelRouteAPI.Infrastructure.Cache;

public class RouteCache
{
    public HashSet<int> IdRoute { get; set; } = new();
    public Dictionary<string, SortedDictionary<decimal, int>> PriceIndex { get; set; }

    public Dictionary<string, SortedDictionary<DateTime, int>> TimeLimitIndex { get; set; }
    public Dictionary<string, List<Route>> DestinationDateIndex { get; set; } = new();
}