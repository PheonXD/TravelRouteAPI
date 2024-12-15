using AutoMapper;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using TravelRouteAPI.Domain.Interfaces;
using TravelRouteAPI.Domain.Models;
using TravelRouteAPI.Infrastructure.Configs.Options;
using TravelRouteAPI.Shared.Models;

namespace TravelRouteAPI.Infrastructure.FlightProviders
{
    public class ProviderOneSearchService : IProviderSearchService
    {
        private readonly HttpClient _httpClient;
        private readonly ProviderUrlsOptions _providerUrlsOptions;
        private readonly IMapper _mapper;

        public ProviderOneSearchService(HttpClient httpClient,
            IOptions<ProviderUrlsOptions> providerUrlsOptions,
            IMapper mapper)
        {
            _httpClient = httpClient;
            _providerUrlsOptions = providerUrlsOptions.Value ?? throw new ArgumentNullException(nameof(ProviderUrlsOptions));
            _mapper = mapper;
        }

        public string Name => "ProviderOne";

        public async Task<List<Route>> SearchFlightsAsync(SearchRequest request)
        {
            var providerRequest = _mapper.Map<ProviderOneSearchModel>(request);

            var response = await _httpClient.PostAsJsonAsync(
                _providerUrlsOptions.ProviderUrlOne + "/search",
                providerRequest
            );

            response.EnsureSuccessStatusCode();
            var providerResponse = await response.Content.ReadFromJsonAsync<ProviderOneSearchResponse>();

            return _mapper.Map<List<Route>>(providerResponse.Routes);
        }

        public async Task<bool> CheckHealthAsync()
        {
            var response = await _httpClient.GetAsync(_providerUrlsOptions.ProviderUrlOne + "/ping");
            return response.IsSuccessStatusCode;
        }
    }
}
