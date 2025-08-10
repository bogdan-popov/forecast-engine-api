using Microsoft.EntityFrameworkCore;
using WeatherApi.Data;
using WeatherApi.Models;

namespace WeatherApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _cotext;

    public UserRepository(DataContext cotext)
    {
        _cotext = cotext;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _cotext.Users.FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _cotext.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _cotext.Users.ToListAsync();
    }

    public async Task AddAsync(User entity)
    {
        await _cotext.Users.AddAsync(entity);
    }

    public void Delete(User entity)
    {
        _cotext.Users.Remove(entity);
    }

    public void Update(User entity)
    {
        _cotext.Users.Update(entity);
    }
}
