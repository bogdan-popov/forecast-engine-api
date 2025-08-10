using WeatherApi.Models;

namespace WeatherApi.Services
{
    public interface IWeatherService
    {
        Task<Weather?> GetWeatherAsync(string city);
        Task<List<Weather>> GetAllWeathersAsync(List<string> cities);
        Task<List<Weather>> GetWeatherForFavoritesAsync(int userId);
    }
}
