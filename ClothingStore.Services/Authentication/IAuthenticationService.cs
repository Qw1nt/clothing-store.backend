using ClothingStore.Data.Requests;
using ClothingStore.Data.Responses;

namespace ClothingStore.Services.Authentication;

public interface IAuthenticationService
{
    Task<RegisterResponse> Register(RegisterRequest request, string? role = null);

    Task<LoginResponse> Login(IdentityRequest request);
}