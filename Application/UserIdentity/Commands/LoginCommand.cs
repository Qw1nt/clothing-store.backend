using Application.Common.Contracts;
using Domain.Common;
using Mediator;

namespace Application.UserIdentity.Commands;

public record LoginCommand(string Login, string Password) : ICommand<OperationResult>;

public class LoginCommandHandler : ICommandHandler<LoginCommand, OperationResult>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async ValueTask<OperationResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.Login(command);
        return result.Success ? OperationResult.Ok(result) : OperationResult.NotFound(result.Error);
    }
}
