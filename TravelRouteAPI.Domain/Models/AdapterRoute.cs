using System.Runtime.CompilerServices;

namespace TravelRouteAPI.Shared.Models
{
    public class AdapterRoute : Route
    {
        public string Adapter { get; set; }

        public string GetAdapterRouteCacheKey()
        {
            return Adapter + "." + Id;
        }
    }
}
