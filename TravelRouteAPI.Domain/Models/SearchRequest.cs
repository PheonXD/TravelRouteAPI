namespace TravelRouteAPI.Shared.Models;

public class SearchRequest
{
    /// <summary>
    /// Start point of route, e.g. Moscow
    /// </summary>
    public required string Origin { get; set; }

    /// <summary>
    /// End point of route, e.g. Sochi
    /// </summary>
    public required string Destination { get; set; }

    /// <summary>
    /// Start date of route
    /// </summary>
    public DateTime OriginDateTime { get; set; }

    /// <summary>
    /// Filter to find routes
    /// </summary>
    public SearchFilters? Filters { get; set; }

    public string GetRouteCacheKey()
    {
        return Origin + Destination + OriginDateTime.ToString();
    }
}