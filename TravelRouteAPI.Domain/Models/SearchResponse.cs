using TravelRouteAPI.Shared.Models;

namespace TravelRouteAPI.Domain.Models;

public class SearchResponse
{
    /// <summary>
    /// Array of routes
    /// </summary>
    public required Route[] Routes { get; set; }

    /// <summary>
    /// The cheapest route
    /// </summary>
    public decimal MinPrice { get; set; }

    /// <summary>
    /// Most expensive route
    /// </summary>
    public decimal MaxPrice { get; set; }

    /// <summary>
    /// The fastest route
    /// </summary>
    public int MinMinutesRoute { get; set; }

    /// <summary>
    /// The longest route
    /// </summary>
    public int MaxMinutesRoute { get; set; }
}