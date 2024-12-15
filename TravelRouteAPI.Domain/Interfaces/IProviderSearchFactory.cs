using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelRouteAPI.Domain.Enums;

namespace TravelRouteAPI.Domain.Interfaces;

public interface IProviderSearchFactory
{
    IProviderSearchService CreateProvider(FlightProvider providerType);
}
