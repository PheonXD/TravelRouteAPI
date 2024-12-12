using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TravelRouteAPI.Infrastructure.Configs.Options;

namespace TravelRouteAPI.Infrastructure.Configs.Setup;

public class FusionCacheOptionsSetup : IConfigureOptions<FusionCacheOptions>
{
    private const string SectionName = "FusionCache";

    private readonly IConfiguration _configuration;

    public FusionCacheOptionsSetup(IConfiguration configuration)
     => _configuration = configuration;

    public void Configure(FusionCacheOptions options)
        => _configuration.GetRequiredSection(SectionName)
            .Bind(options);
}