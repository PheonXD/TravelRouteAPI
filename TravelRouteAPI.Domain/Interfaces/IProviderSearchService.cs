using TravelRouteAPI.Shared.Models;

namespace TravelRouteAPI.Domain.Interfaces;

public interface IProviderSearchService
{

    Task<List<Route>> SearchFlightsAsync(SearchRequest request);

    Task<bool> CheckHealthAsync();
}