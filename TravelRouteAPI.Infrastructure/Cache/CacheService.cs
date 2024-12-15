using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TravelRouteAPI.Domain.Interfaces;
using TravelRouteAPI.Domain.Models;
using TravelRouteAPI.Infrastructure.Configs.Options;
using TravelRouteAPI.Shared.Models;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Events;

namespace TravelRouteAPI.Infrastructure.Cache;

public class CacheService : ICacheService
{
    private readonly IFusionCache _cache;
    private readonly CacheOptions _cacheOptions;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IFusionCache cache,
        IOptions<CacheOptions> cacheOptions,
        ILogger<CacheService> logger)
    {
        _cache = cache;
        _cache.Events.Set += OnRouteDirectionKeysSetEvent;
        _cache.Events.Expire += OnRouteExpireEvent;

        _cacheOptions = cacheOptions.Value ?? throw new ArgumentNullException(nameof(cacheOptions));

        _logger = logger;
    }

    public async Task AddRouteAsync(Route route)
    {
        var flightKey = GetCombinedCacheKey(DataCacheKeys.Routes, route.Id.ToString());
        var ttl = route.TimeLimit - DateTime.UtcNow;

        await _cache.SetAsync(flightKey, route, ttl);

        _logger.LogInformation("New routes was added success");
    }

    public async IAsyncEnumerable<Route> SearchRoutesAsync(string origin, string destination, DateTime originDateTime)
    {
        var key = GetCombinedCacheKey(DataCacheKeys.RouteDirection, origin + destination + originDateTime.ToString());
        var routeKeys = await _cache.GetOrDefaultAsync(key, new HashSet<string>());

        foreach (var routeKey in routeKeys)
        {
            var result = await _cache.TryGetAsync<Route>(routeKey);
            if (result.HasValue)
            {
                yield return result;
            }
        }
    }

    /// <summary>
    /// Обработка события истечения TTL маршрута.
    /// </summary>
    private async void OnRouteExpireEvent(object? sender, FusionCacheEntryEventArgs e)
    {
        _logger.LogInformation("OnRouteExpireEvent started");

        if (!e.Key.Contains(DataCacheKeys.Routes))
            return;

        var routeResult = await _cache.TryGetAsync<Route>(e.Key);
        if (!routeResult.HasValue)
            return;

        var route = routeResult.Value;
        var directionKey = GetCombinedCacheKey(DataCacheKeys.RouteDirection, route.GetRouteDirectionCacheKey());
        var directionCache = await _cache.GetOrSetAsync(directionKey, _ => Task.FromResult(new HashSet<string>()), _cacheOptions.DefaultTTL);

        if (directionCache.Remove(route.Id.ToString()))
        {
            await _cache.SetAsync(directionKey, directionCache, _cacheOptions.DefaultTTL);

            _logger.LogInformation($"Route with id {route.Id} was removed from cache (reason: TTL expired)");
        }

        _logger.LogInformation("OnRouteExpireEvent ended");
    }

    /// <summary>
    /// Обработка события добавления ключей маршрутов.
    /// </summary>
    private async void OnRouteDirectionKeysSetEvent(object? sender, FusionCacheEntryEventArgs e)
    {
        if (!e.Key.Contains(DataCacheKeys.RouteDirection))
            return;

        var directionKeys = await _cache.GetOrSetAsync(
            DataCacheKeys.RouteDirectionKeys,
            _ => Task.FromResult(new HashSet<string>()),
            _cacheOptions.DefaultTTL
        );

        if (directionKeys.Add(e.Key))
        {
            await _cache.SetAsync(DataCacheKeys.RouteDirectionKeys, directionKeys, _cacheOptions.DefaultTTL);
        }
    }

    private string GetCombinedCacheKey(string cacheKey, string subKey)
    {
        return $"{cacheKey}:{subKey}";
    }
}