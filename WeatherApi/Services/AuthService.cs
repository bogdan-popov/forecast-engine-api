using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using WeatherApi.Models;
using Microsoft.EntityFrameworkCore;
using WeatherApi.Data;
using Microsoft.Extensions.Options;
using WeatherApi.Configuration;
using Microsoft.Extensions.Logging;
using WeatherApi.Exceptions;
using WeatherApi.Repositories;

namespace WeatherApi.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly DataContext _context;
    private readonly JwtSettings _settings;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, DataContext context, IOptions<JwtSettings> settings, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _context = context;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<int> Register(UserRegisterDto request)
    {
        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            Username = request.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await _userRepository.AddAsync(user);

        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task<string> Login(UserRegisterDto request)
    {
        _logger.LogInformation("Попытка входа для пользователя {Username}", request.Username);

        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user == null)
        {
            _logger.LogWarning("Неудачная попытка входа: пользователь {Username} не найден", request.Username);
            throw new BadRequestException("Неверные учетные данные.");
        }

        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            _logger.LogWarning("Неудачная попытка входа: неверный пароль для пользователя {Username}", request.Username);
            throw new BadRequestException("Неверные учетные данные.");
        }

        string token = CreateToken(user);
        _logger.LogInformation("Пользователь {Username} успешно вошел в систему", request.Username);
        return token;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computerHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computerHash.SequenceEqual(passwordHash);
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_settings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
