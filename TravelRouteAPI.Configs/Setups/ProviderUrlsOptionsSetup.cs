using TravelRouteAPI.Infrastructure.Configs.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace TravelRouteAPI.Infrastructure.Configs.Setup;

public class ProviderUrlsOptionsSetup : IConfigureOptions<ProviderUrlsOptions>
{
    private const string SectionName = "ProviderUrls";

    private readonly IConfiguration _configuration;

    public ProviderUrlsOptionsSetup(IConfiguration configuration)
     => _configuration = configuration;

    public void Configure(ProviderUrlsOptions options)
        => _configuration.GetRequiredSection(SectionName)
            .Bind(options);
}
