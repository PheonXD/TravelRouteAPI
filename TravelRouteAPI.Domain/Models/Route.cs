using System.Runtime.CompilerServices;

namespace TravelRouteAPI.Shared.Models
{
    public class Route
    {
        /// <summary>
        /// Identifier of the whole route
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Start point of route
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// End point of route
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Start date of route
        /// </summary>
        public DateTime OriginDateTime { get; set; }

        /// <summary>
        /// End date of the route and date of destination
        /// </summary>
        public DateTime DestinationDateTime { get; set; }

        /// <summary>
        /// Price of route
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Time limit after it expires, route became not actual
        /// </summary>
        public DateTime TimeLimit { get; set; }

        public string GetRouteDirectionCacheKey()
        {
            return Origin + Destination + OriginDateTime.ToString();
        }

        // Переопределяем методы Equals и GetHashCode.
        public override bool Equals(object? obj)
        {
            if (obj is null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Route)obj;

            return Id == other.Id &&
                   Origin == other.Origin &&
                   Destination == other.Destination &&
                   OriginDateTime == other.OriginDateTime;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id, Origin, Destination, OriginDateTime).GetHashCode();
            }
        }
    }
}
