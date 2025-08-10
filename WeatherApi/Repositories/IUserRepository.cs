using WeatherApi.Models;

namespace WeatherApi.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
}

