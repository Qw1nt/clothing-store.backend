using Application;
using ClothingStore.Data.Responses;

namespace Infrastructure.Services.Authentication;

public interface IAuthenticationService
{
    Task<RegisterResponse> Register(RegisterRequest request, string? role = null);

    Task<LoginResponse> Login(IdentityRequest request);
}