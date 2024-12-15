using Microsoft.AspNetCore.Mvc;
using TravelRouteAPI.Domain.Interfaces;
using TravelRouteAPI.Domain.Models;
using TravelRouteAPI.Shared.Models;

namespace TravelRouteAPI.Domain.Controllers;

/// <summary>
/// Controller for aggregated search of flight routes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RoutesController : ControllerBase
{
    private readonly ISearchService _searchService;

    public RoutesController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    /// <summary>
    /// Performs a search for routes using providers and cache.
    /// </summary>
    /// <param name="request">The search request parameters.</param>
    /// <returns>A list of routes matching the criteria.</returns>
    [HttpPost("search")]
    public async Task<IActionResult> SearchRoutes([FromBody] SearchRequest request)
    {
        if (request.Filters?.OnlyCached == true)
        {
            var cachedRoutes = await _searchService.SearchInCacheAsync(request);
            return Ok(new SearchResponse { Routes = cachedRoutes.ToArray() });
        }

        var result = await _searchService.SearchAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Checks the availability of the API and its providers.
    /// </summary>
    /// <returns>A dictionary with the health status of each provider.</returns>
    [HttpGet("availability")]
    public async Task<IActionResult> CheckAvailability()
    {
        var availability = await _searchService.CheckProvidersAvailabilityAsync();
        return Ok(availability);
    }
}
