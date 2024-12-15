using Microsoft.Extensions.DependencyInjection;
using TravelRouteAPI.Domain.Enums;
using TravelRouteAPI.Domain.Interfaces;

namespace TravelRouteAPI.Infrastructure.FlightProviders
{
    public class ProviderSearchFactory : IProviderSearchFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ProviderSearchFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IProviderSearchService CreateProvider(FlightProvider providerType)
        {
            return providerType switch
            {
                FlightProvider.ProviderOne => _serviceProvider.GetRequiredService<ProviderOneSearchService>(),
                FlightProvider.ProviderTwo => _serviceProvider.GetRequiredService<ProviderTwoSearchService>(),
                _ => throw new ArgumentException($"Provider '{providerType}' is not supported.")
            };
        }
    }
}
