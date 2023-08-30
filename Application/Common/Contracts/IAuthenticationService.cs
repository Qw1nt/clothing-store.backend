using Application.UserIdentity.Commands;
using Domain.Common;

namespace Application.Common.Contracts;

public interface IAuthenticationService
{
    Task<IdentityKeyPair> Register(RegisterCommand command, string? role = null);

    Task<IdentityKeyPair> Login(LoginCommand command);
}