using AutoMapper;
using TravelRouteAPI.Domain.Models;
using TravelRouteAPI.Shared.Models;

namespace TravelRouteAPI.Infrastructure.Mapping;

// Настройка маппинга
public class SearchRequestMappingProfile : Profile
{
    public SearchRequestMappingProfile()
    {
        // ProviderOne -> Route
        CreateMap<ProviderOneRoute, Route>()
            .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.From))
            .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.To))
            .ForMember(dest => dest.OriginDateTime, opt => opt.MapFrom(src => src.DateFrom))
            .ForMember(dest => dest.DestinationDateTime, opt => opt.MapFrom(src => src.DateTo))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.TimeLimit, opt => opt.MapFrom(src => src.TimeLimit));

        // ProviderTwo -> Route
        CreateMap<ProviderTwoRoute, Route>()
            .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Departure.Point))
            .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Arrival.Point))
            .ForMember(dest => dest.OriginDateTime, opt => opt.MapFrom(src => src.Departure.Date))
            .ForMember(dest => dest.DestinationDateTime, opt => opt.MapFrom(src => src.Arrival.Date))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.TimeLimit, opt => opt.MapFrom(src => src.TimeLimit));

        CreateMap<SearchRequest, ProviderOneSearchModel>()
            .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.Origin))
            .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.Destination))
            .ForMember(dest => dest.DateFrom, opt => opt.MapFrom(src => src.OriginDateTime))
            .ForMember(dest => dest.DateTo, opt => opt.MapFrom(src => src.Filters != null ? src.Filters.DestinationDateTime : null))
            .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src => src.Filters != null ? src.Filters.MaxPrice : null));

        // SearchRequest -> ProviderTwoSearchRequest
        CreateMap<SearchRequest, ProviderTwoSearchModel>()
            .ForMember(dest => dest.Departure, opt => opt.MapFrom(src => src.Origin))
            .ForMember(dest => dest.Arrival, opt => opt.MapFrom(src => src.Destination))
            .ForMember(dest => dest.DepartureDate, opt => opt.MapFrom(src => src.OriginDateTime))
            .ForMember(dest => dest.MinTimeLimit, opt => opt.MapFrom(src => src.Filters != null ? src.Filters.MinTimeLimit : null));

        // ProviderOneSearchResponse -> SearchResponse
        CreateMap<ProviderOneSearchResponse, SearchResponse>()
            .ForMember(dest => dest.Routes, opt => opt.MapFrom(src => src.Routes));

        // ProviderTwoSearchResponse -> SearchResponse
        CreateMap<ProviderTwoSearchResponse, SearchResponse>()
            .ForMember(dest => dest.Routes, opt => opt.MapFrom(src => src.Routes));
    }
}

