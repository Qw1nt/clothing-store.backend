using ClothingStore.Configurations;
using ClothingStore.Data.Context;
using ClothingStore.Data.Context.Entities;
using ClothingStore.Data.Requests;
using ClothingStore.Data.Responses;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHashSaltService _hashSaltService;
    private readonly JwtGenerationService _jwtGenerationService;
    private readonly DataContext _dataContext;

    public AuthenticationService(IHashSaltService hashSaltService, JwtGenerationService jwtGenerationService, DataContext dataContext)
    {
        _hashSaltService = hashSaltService;
        _dataContext = dataContext;
        _jwtGenerationService = jwtGenerationService;
    }
    
    public async Task<RegisterResponse> Register(RegisterRequest request, string? role = null)
    {
        User user = new()
        {
            Login = request.Login,
            PasswordHash = request.Password,
            Salt = _hashSaltService.Salt(),
            Role =  role ?? IdentityConfiguration.Roles.User 
        };
        
        user.PasswordHash = _hashSaltService.Hash(user.PasswordHash, user.Salt);

        await _dataContext.Users.AddAsync(user);
        await _dataContext.SaveChangesAsync();
        
        return new RegisterResponse {Success = true};
    }

    public async Task<LoginResponse> Login(IdentityRequest request)
    {
        var user = await _dataContext.Users
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