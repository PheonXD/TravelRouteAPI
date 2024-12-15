using TravelRouteAPI.Domain.Enums;
using TravelRouteAPI.Domain.Models;
using TravelRouteAPI.Shared.Models;

namespace TravelRouteAPI.Domain.Interfaces;

public interface ISearchService
{
    /// <summary>
    /// Выполняет полный поиск маршрутов с использованием провайдеров.
    /// </summary>
    Task<SearchResponse> SearchAsync(SearchRequest request);

    /// <summary>
    /// Выполняет поиск только в рамках закэшированных данных.
    /// </summary>
    Task<List<Route>> SearchInCacheAsync(SearchRequest request);

    /// <summary>
    /// Проверяет доступность провайдеров.
    /// </summary>
    Task<Dictionary<FlightProvider, bool>> CheckProvidersAvailabilityAsync();
}
