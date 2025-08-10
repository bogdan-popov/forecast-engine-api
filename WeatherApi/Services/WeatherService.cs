using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using WeatherApi.Models;
using Microsoft.Extensions.Options;
using WeatherApi.Configuration;
using WeatherApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace WeatherApi.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _client = new();
    private readonly WeatherApiSettings _settings;
    private readonly DataContext _context;
    private readonly IMemoryCache _cache;

    public WeatherService(IOptions<WeatherApiSettings> settings, DataContext context, IMemoryCache cache)
    {
        _settings = settings.Value;
        _context = context;
        _cache = cache;
    }


    public async Task<Weather?> GetWeatherAsync(string city)
    {
        var cacheKey = $"weather_{city.ToLower()}";

        if (_cache.TryGetValue(cacheKey, out Weather? cachedWeather))
        {
            return cachedWeather;
        }


        try
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_settings.ApiKey}&units=metric&lang=ru";
            string jsonResponse = await _client.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<WeatherApiResponse>(jsonResponse);

            var weather = new Weather
            {
                City = apiResponse.CityName,
                Temperature = apiResponse.Main.Temperature,
                Humidity = apiResponse.Main.Humidity,
                Description = apiResponse.Weather[0].Description,
            };

            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

            _cache.Set(cacheKey, weather, cacheOptions);

            return weather;

        } catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<List<Weather>> GetAllWeathersAsync(List<string> cities)
    {
        try
        {
            if (cities.Count == 0)
            {
                return null;
            }

            var weatherTasks = new List<Task<Weather?>>();
            foreach (var city in cities)
            {
                weatherTasks.Add(GetWeatherAsync(city));
            }

            var weatherResults = await Task.WhenAll(weatherTasks);

            List<Weather> weathers = weatherResults.Where(w => w != null).ToList();

            return weathers;
        } catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<List<Weather>> GetWeatherForFavoritesAsync(int userId)
    {
        var favoritesCityNames = await _context.FavoriteCities
            .Where(fc => fc.UserId == userId)
            .Select(fc => fc.Name)
            .ToListAsync();

        if (favoritesCityNames.Count == 0)
        {
            return new List<Weather>();
        }

        var weathers = await GetAllWeathersAsync(favoritesCityNames);

        return weathers;
    }
}
