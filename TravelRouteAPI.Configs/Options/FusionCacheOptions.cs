namespace TravelRouteAPI.Infrastructure.Configs.Options
{
    public class FusionCacheOptions
    {
        public required string DefaultTTL { get; set; }

        public required string CleanupInterval { get; set; }
    }
}
