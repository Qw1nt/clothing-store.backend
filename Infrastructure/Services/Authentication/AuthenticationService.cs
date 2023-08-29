using Application;
using Application.Common.Contracts;
using ClothingStore.Data.Responses;
using Domain;
using Domain.Common.Configurations;
using Domain.Entities;
using Infrastructure.Services.HashSalt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHashSaltService _hashSaltService;
    private readonly JwtGenerationService _jwtGenerationService;
    private readonly IApplicationDataContext _applicationDataContext;

    public AuthenticationService(IHashSaltService hashSaltService, JwtGenerationService jwtGenerationService, IApplicationDataContext applicationDataContext)
    {
        _hashSaltService = hashSaltService;
        _applicationDataContext = applicationDataContext;
        _jwtGenerationService = jwtGenerationService;
    }
    
    public async Task<RegisterResponse> Register(RegisterRequest request, string? role = null)
    {
        User user = new()
        {
            Login = request.Login,
            PasswordHash = request.Password,
            Salt = _hashSaltService.Salt(),
            Role =  role ?? IdentityConfiguration.Roles.User,
            RegisterDate = DateTime.UtcNow
        };
        
        user.PasswordHash = _hashSaltService.Hash(user.PasswordHash, user.Salt);

        await _applicationDataContext.Users.AddAsync(user);
        await _applicationDataContext.SaveChangesAsync();
        
        return new RegisterResponse {Success = true};
    }

    public async Task<LoginResponse> Login(IdentityRequest request)
    {
        var user = await _applicationDataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == request.Login);

        if (user is null || user.PasswordHash != _hashSaltService.Hash(request.Password, user.Salt))
            return FailureLogin("Неверное имя пользователя или пароль");

        var claims = _jwtGenerationService.AssembleClaimsIdentity(user);
        return new LoginResponse {Success = true, AccessToken = _jwtGenerationService.GenerateJwtToken(claims)};
    }

    private RegisterResponse FailureRegister(string error) => new() {Success = false, Error = error};

    private LoginResponse FailureLogin(string error) => new() {Success = false, Error = error};
}