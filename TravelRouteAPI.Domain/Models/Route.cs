namespace TravelRouteAPI.Shared.Models
{
    public class Route
    {
        /// <summary>
        /// Identifier of the whole route
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Start point of route
        /// </summary>
        public required string Origin { get; set; }

        /// <summary>
        /// End point of route
        /// </summary>
        public required string Destination { get; set; }

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
    }
}
