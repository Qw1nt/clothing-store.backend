using Application.Common.Contracts;
using Domain.Common;
using Mediator;

namespace Application.UserIdentity.Commands;

public record RegisterWithRoleCommand(string Role, RegisterCommand Data) : ICommand<OperationResult>;

public class RegisterWithRoleCommandHandler : ICommandHandler<RegisterWithRoleCommand, OperationResult>
{
    private readonly IAuthenticationService _authenticationService;

    public RegisterWithRoleCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async ValueTask<OperationResult> Handle(RegisterWithRoleCommand command, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.Register(command.Data, command.Role);
        return result.Success ? OperationResult.Ok(result.AccessToken) : OperationResult.BadRequest(result.Error);
    }
}