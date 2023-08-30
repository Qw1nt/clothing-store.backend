using Application.Common.Contracts;
using Domain.Common;
using Mediator;

namespace Application.UserIdentity.Commands;

public record RegisterCommand(string Login, string Password) : ICommand<OperationResult>;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, OperationResult>
{
    private readonly IAuthenticationService _authenticationService;
    
    public RegisterCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async ValueTask<OperationResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.Register(command);
        return result.Success ? OperationResult.Ok(result.AccessToken) : OperationResult.BadRequest(result.Error);
    }
}
