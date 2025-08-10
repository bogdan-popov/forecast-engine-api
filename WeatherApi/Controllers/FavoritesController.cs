using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WeatherApi.Data;
using WeatherApi.Models;

namespace WeatherApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FavoritesController : Controller
{
    private readonly DataContext _context;
    private readonly ILogger<FavoritesController> _logger;

    public FavoritesController(DataContext context, ILogger<FavoritesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            var ex = new Exception("Claim NameIdentifier не найден в токене.");
            _logger.LogError(ex, "Критическая ошибка: не удалось определить ID пользователя.");
            throw ex;
        }

        return int.Parse(userIdClaim.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetFavoriteCities()
    {
        var userId = GetCurrentUserId();
        var favoriteCities = await _context.FavoriteCities
            .Where(fc => fc.UserId == userId)
            .Select(fc => fc.Name)
            .ToListAsync();

        return Ok(favoriteCities);
    }

    [HttpPost]
    public async Task<IActionResult> AddFavoriteCity([FromQuery] string cityName)
    {
        if (string.IsNullOrEmpty(cityName))
        {
            return BadRequest("Имя города не может быть пустым.");
        }

        var userId = GetCurrentUserId();

        var existingFavorite = await _context.FavoriteCities.FirstOrDefaultAsync(fc => fc.UserId == userId && fc.Name.ToLower() == cityName.ToLower());

        if (existingFavorite != null)
        {
            return Conflict($"Город '{cityName}' уже в избранном.");
        }

        var newFavorite = new FavoriteCity
        {
            Name = cityName,
            UserId = userId,
        };

        _context.FavoriteCities.Add(newFavorite);
        await _context.SaveChangesAsync();

        return Ok($"Город '{cityName}' успешно добавлен в избранное.");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFavoriteCity([FromQuery] string cityName)
    {
        if (string.IsNullOrEmpty(cityName))
        {
            return BadRequest("Имя города не может быть пустым.");
        }

        var userId = GetCurrentUserId();

        var favoriteToRemove = await _context.FavoriteCities.FirstOrDefaultAsync(fc => fc.UserId == userId && fc.Name.ToLower() == cityName.ToLower());

        if (favoriteToRemove == null)
        {
            return NotFound($"Город '{cityName}' не найден в вашем избранном.");
        }

        _context.FavoriteCities.Remove(favoriteToRemove);
        await _context.SaveChangesAsync();

        return Ok($"Город '{cityName}' успешно удален из избранного.");
    }
}
