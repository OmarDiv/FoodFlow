namespace FoodFlow.Contracts.GeoLocation
{
    public class LocationResult
    {
        public string? Formatted { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? Housenumber { get; set; }
        public string? Postcode { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
