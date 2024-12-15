using TravelRouteAPI.Domain.Enums;
using TravelRouteAPI.Domain.Interfaces;
using TravelRouteAPI.Domain.Models;
using TravelRouteAPI.Shared.Models;
namespace TravelRouteAPI.Infrastructure.Cache;

public class SearchService : ISearchService
{
    private readonly IProviderSearchFactory _providerFactory;
    private readonly ICacheService _cacheService;

    public SearchService(IProviderSearchFactory providerFactory, ICacheService cacheService)
    {
        _providerFactory = providerFactory;
        _cacheService = cacheService;
    }

    /// <summary>
    /// Выполняет полный поиск маршрутов с использованием провайдеров и кэширует результаты.
    /// </summary>
    public async Task<SearchResponse> SearchAsync(SearchRequest request)
    {
        var routes = new List<Route>();

        foreach (FlightProvider providerEnum in Enum.GetValues(typeof(FlightProvider)))
        {
            if (providerEnum == FlightProvider.None)
                continue;

            var provider = _providerFactory.CreateProvider(providerEnum);

            try
            {
                var providerRoutes = await provider.SearchFlightsAsync(request);
                routes.AddRange(providerRoutes);

                // Кэшируем результаты
                foreach (var route in providerRoutes)
                {
                    await _cacheService.AddRouteAsync(route);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with provider '{providerEnum}': {ex.Message}");
            }
        }

        return new SearchResponse { Routes = routes.ToArray() };
    }

    /// <summary>
    /// Выполняет поиск только в рамках закэшированных данных.
    /// </summary>
    public async Task<List<Route>> SearchInCacheAsync(SearchRequest request)
    {
        var cachedRoutes = new List<Route>();

        await foreach (var route in _cacheService.SearchRoutesAsync(request.Origin, request.Destination, request.OriginDateTime))
        {
            cachedRoutes.Add(route);
        }

        return cachedRoutes;
    }

    /// <summary>
    /// Проверяет доступность всех провайдеров.
    /// </summary>
    public async Task<Dictionary<FlightProvider, bool>> CheckProvidersAvailabilityAsync()
    {
        var availability = new Dictionary<FlightProvider, bool>();

        foreach (FlightProvider providerEnum in Enum.GetValues(typeof(FlightProvider)))
        {
            if (providerEnum == FlightProvider.None)
                continue;

            var provider = _providerFactory.CreateProvider(providerEnum);
            availability[providerEnum] = await provider.CheckHealthAsync();
        }

        return availability;
    }
}
