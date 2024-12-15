using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelRouteAPI.Domain.Interfaces;
using TravelRouteAPI.Infrastructure.Cache;
using TravelRouteAPI.Infrastructure.Configs.Options;
using TravelRouteAPI.Infrastructure.Configs.Setup;
using TravelRouteAPI.Infrastructure.FlightProviders;
using TravelRouteAPI.Infrastructure.Mapping;

namespace TravelRouteAPI;

internal static class DependencyInjection
{
    public static void Inject(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterConfigs(configuration)
                .RegisterServices(configuration)
                .RegisterProviders(configuration)
                .AddAutoMapper(typeof(SearchRequestMappingProfile));
    }

    private static IServiceCollection RegisterConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<FusionCacheOptionsSetup>();
        services.ConfigureOptions<ProviderUrlsOptionsSetup>();

        return services;
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }

    private static IServiceCollection RegisterProviders(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetRequiredSection(ProviderUrlsOptionsSetup.SectionName).Get<ProviderUrlsOptions>();

        //Регистрация первого провайдера
        services.AddScoped<IProviderSearchService, ProviderOneSearchService>();
        services.AddHttpClient<IProviderSearchService, ProviderOneSearchService>(client =>
        {
            client.BaseAddress = new Uri(config.ProviderUrlOne);
        });

        //Регистрация второго провайдера
        services.AddScoped<IProviderSearchService, ProviderTwoSearchService>();
        services.AddHttpClient<IProviderSearchService, ProviderTwoSearchService>(client =>
        {
            client.BaseAddress = new Uri(config.ProviderUrlOne);
        });

        // Регистрация фабрики
        services.AddSingleton<IProviderSearchFactory, ProviderSearchFactory>();

        return services;
    }
}
