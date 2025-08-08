using WeatherApi.Models;

namespace WeatherApi.Services;

public interface IAuthService
{
    Task<int> Register(UserRegisterDto request);
    Task<string> Login(UserRegisterDto request);
}
