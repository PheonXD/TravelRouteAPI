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

    public CacheService(IFusionCache cache,
        IOptions<CacheOptions> cacheOptions)
    {
        _cache = cache;
        _cache.Events.Set += RouteDirectionKeysSetEvent;
        _cache.Events.Expire += RouteExpireEvent;

        _cacheOptions = cacheOptions.Value ?? throw new ArgumentNullException(nameof(cacheOptions));
    }

    public async Task AddRouteAsync(AdapterRoute route)
    {
        var routeKey = route.GetAdapterRouteCacheKey();

        var flightKey = GetCombinedCacheKey(DataCacheKeys.Routes, routeKey);
        var ttl = route.TimeLimit - DateTime.UtcNow;

        await _cache.SetAsync(flightKey, route, ttl);
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

    private async void RouteExpireEvent(object? sender, FusionCacheEntryEventArgs e)
    {
        if (e.Key.Contains(DataCacheKeys.Routes))
        {
            var route = await _cache.TryGetAsync<AdapterRoute>(e.Key);
            if (!route.HasValue)
            {
                return;
            }

            var directionKey = GetCombinedCacheKey(DataCacheKeys.RouteDirection, route.Value.GetRouteDirectionCacheKey());
            var directionCache = await _cache.GetOrSetAsync(
                directionKey,
                _ => Task.FromResult(new HashSet<string>()),
                _cacheOptions.DefaultTTL);

            if (directionCache.Count > 0)
            {
                var routeKey = route.Value.GetAdapterRouteCacheKey();
                directionCache.Remove(routeKey);
                await _cache.SetAsync(directionKey, directionCache);
            }


        }
    }

    private async void RouteDirectionKeysSetEvent(object? sender, FusionCacheEntryEventArgs e)
    {
        if (e.Key.Contains(DataCacheKeys.RouteDirection))
        {
            var directionCacheKeys = await _cache.GetOrSetAsync(
                DataCacheKeys.RouteDirectionKeys,
                _ => Task.FromResult(new HashSet<string>()),
                _cacheOptions.DefaultTTL
            );

            // Добавляем новый ключ в HashSet
            if (directionCacheKeys.Add(e.Key))
            {
                // Явно обновляем значение в кэше
                await _cache.SetAsync(DataCacheKeys.RouteDirectionKeys, directionCacheKeys, _cacheOptions.DefaultTTL);
            }
        }
    }

    private string GetCombinedCacheKey(string cacheKey, string subKey)
    {
        return $"{cacheKey}:{subKey}";
    }
}