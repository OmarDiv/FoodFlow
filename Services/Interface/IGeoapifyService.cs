using FoodFlow.Contracts.GeoLocation;

namespace FoodFlow.Services.Interface
{
    public interface IGeoapifyService
    {
        Task<LocationResult?> GetAddressFromCoordinatesAsync(double latitude, double longitude);
    }
}
