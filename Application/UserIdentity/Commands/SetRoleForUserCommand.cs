using Application.Common.Contracts;
using Domain.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.UserIdentity.Commands;

public record SetRoleForUserCommand(int UserId, string Role = null!) : ICommand<OperationResult>;

public class SetRoleForUserCommandHandler : ICommandHandler<SetRoleForUserCommand, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public SetRoleForUserCommandHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(SetRoleForUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _applicationDataContext.Users
            .SingleOrDefaultAsync(x => x.Id == command.UserId, cancellationToken: cancellationToken);

        if (user == null)
            return OperationResult.NotFound();
        
        user.Role = command.Role;
        _applicationDataContext.Users.Update(user);
        await _applicationDataContext.SaveChangesAsync(cancellationToken);
        
        return OperationResult.Ok(user);

    }
}