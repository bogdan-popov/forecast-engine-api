using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;
using System.Security.Claims;

namespace WeatherApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    private int GetCurrentUserId()
    {
        return int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentWeather([FromQuery] string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return BadRequest("Параметр 'city' не может быть пустым!");
        }

        var weather = await _weatherService.GetWeatherAsync(city);

        if (weather == null)
        {
            return NotFound($"Не удалось найти погоду для горда {city}");
        }

        return Ok(weather);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllWeathers([FromQuery] List<string> cities)
    {
        if (cities.Count == 0)
        {
            return BadRequest("Список городов не может быть пустым!");
        }

        var weathers = await _weatherService.GetAllWeathersAsync(cities);

        if (weathers == null)
        {
            return NotFound($"Не удалось найти погоду для ваших городов");
        }

        return Ok(weathers);
    }

    [HttpGet("favorites")]
    public async Task<IActionResult> GetWeatherForFavoriteCities()
    {
        var userId = GetCurrentUserId();
        var weathers = await _weatherService.GetWeatherForFavoritesAsync(userId);

        return Ok(weathers);
    }
}
