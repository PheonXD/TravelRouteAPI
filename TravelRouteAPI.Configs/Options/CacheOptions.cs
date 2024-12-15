namespace TravelRouteAPI.Infrastructure.Configs.Options
{
    public class CacheOptions
    {
        public required TimeSpan DefaultTTL { get; set; }

        public required int CleanupMinutesInterval { get; set; }
    }
}
