using Microsoft.EntityFrameworkCore;
using WeatherApi.Models;

namespace WeatherApi.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<FavoriteCity> FavoriteCities { get; set; }
}
