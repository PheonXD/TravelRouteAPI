namespace TravelRouteAPI.Shared.Models;
public class SearchFilters
{
    /// <summary>
    /// End date of route
    /// </summary>
    public DateTime? DestinationDateTime { get; set; }

    /// <summary>
    /// Maximum price of route
    /// </summary>
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// Minimum value of time limit for route
    /// </summary>
    public DateTime? MinTimeLimit { get; set; }

    /// <summary>
    /// Forcibly search in cached data
    /// </summary>
    public bool? OnlyCached { get; set; }
}