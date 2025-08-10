namespace WeatherApi.Models;

public class FavoriteCity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
