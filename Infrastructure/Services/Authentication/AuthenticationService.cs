using Application.Common.Contracts;
using Application.UserIdentity.Commands;
using Domain.Common;
using Domain.Common.Configurations;
using Domain.Entities;
using Infrastructure.Services.HashSalt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHashSaltService _hashSaltService;
    private readonly JwtGenerationService _jwtGenerationService;
    private readonly IApplicationDataContext _applicationDataContext;
    private readonly IMemoryCache _memoryCache;

    public AuthenticationService(IHashSaltService hashSaltService, JwtGenerationService jwtGenerationService, IApplicationDataContext applicationDataContext, IMemoryCache memoryCache)
    {
        _hashSaltService = hashSaltService;
        _applicationDataContext = applicationDataContext;
        _memoryCache = memoryCache;
        _jwtGenerationService = jwtGenerationService;
    }
    
    public async Task<IdentityKeyPair> Register(RegisterCommand command, string? role = null)
    {
        var canRegister = await RegisterValidation(command);
        if (canRegister.isSuccessful == false)
            return new IdentityKeyPair(false, string.Empty, canRegister.error);
        
        _memoryCache.Set(command.Login, command);
        
        User user = new()
        {
            Login = command.Login,
            PasswordHash = command.Password,
            Salt = _hashSaltService.Salt(),
            Role =  role ?? IdentityConfiguration.Roles.User,
            RegisterDate = DateTime.UtcNow
        };
        
        user.PasswordHash = _hashSaltService.Hash(user.PasswordHash, user.Salt);

        await _applicationDataContext.Users.AddAsync(user);
        await _applicationDataContext.SaveChangesAsync();
        _memoryCache.Remove(command.Login);

        return new IdentityKeyPair(true, "", "");
    }

    public async Task<IdentityKeyPair> Login(LoginCommand command)
    {
        var user = await _applicationDataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == command.Login);

        if (user is null || user.PasswordHash != _hashSaltService.Hash(command.Password, user.Salt))
            return FailureLogin("Неверное имя пользователя или пароль");

        var claims = _jwtGenerationService.AssembleClaimsIdentity(user);
        return new IdentityKeyPair(true, _jwtGenerationService.GenerateJwtToken(claims));
    }
    
    private async Task<(bool isSuccessful, string? error)> RegisterValidation(RegisterCommand command)
    {
        if (_memoryCache.TryGetValue(command.Login, out object? value) == true)
            return new ValueTuple<bool, string?>(false, "Error");

        if (await _applicationDataContext.Users.AnyAsync(x => x.Login == command.Login))
            return new ValueTuple<bool, string?>(false, "Пользователь с таким логином уже зарегистрирован");

        return new ValueTuple<bool, string?>(true, "Success");
    }
    
    private IdentityKeyPair FailureLogin(string error) => new(false, string.Empty, error);
}