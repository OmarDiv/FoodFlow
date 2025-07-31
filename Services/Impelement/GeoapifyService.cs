using FoodFlow.Contracts.GeoLocation;
using FoodFlow.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FoodFlow.Services.Implement
{
    public class GeoapifyService(HttpClient httpClient, IOptions<GeoapifySettings> options, ILogger<GeoapifyService> logger) : IGeoapifyService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<GeoapifyService> _logger = logger;
        private readonly string _apiKey = options.Value.ApiKey;

        public async Task<LocationResult?> GetAddressFromCoordinatesAsync(double latitude, double longitude)
        {
            _logger.LogInformation("Fetching address for coordinates: {Latitude}, {Longitude}", latitude, longitude);

            var url = $"https://api.geoapify.com/v1/geocode/reverse?lat={latitude}&lon={longitude}&format=json&apiKey={_apiKey}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Geoapify API returned non-success status code: {StatusCode}", response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                using var json = JsonDocument.Parse(content);

                if (!json.RootElement.TryGetProperty("results", out var results) || results.GetArrayLength() == 0)
                {
                    _logger.LogWarning("Geoapify API returned no results for coordinates: {Latitude}, {Longitude}", latitude, longitude);
                    return null;
                }

                var root = results[0];

                return new LocationResult
                {
                    Formatted = root.GetProperty("formatted").GetString(),
                    Country = root.GetProperty("country").GetString(),
                    State = root.GetProperty("state").GetString(),
                    City = root.GetProperty("city").GetString(),
                    Street = root.TryGetProperty("street", out var street) ? street.GetString() : null,
                    Housenumber = root.TryGetProperty("housenumber", out var house) ? house.GetString() : null,
                    Postcode = root.TryGetProperty("postcode", out var post) ? post.GetString() : null,
                    Lat = root.GetProperty("lat").GetDouble(),
                    Lon = root.GetProperty("lon").GetDouble()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching address from Geoapify for coordinates: {Latitude}, {Longitude}", latitude, longitude);
                return null;
            }
        }
    }
}