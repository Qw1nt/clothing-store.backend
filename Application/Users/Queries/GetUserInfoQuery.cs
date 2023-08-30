using Application.Common.Contracts;
using Domain.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public record GetUserInfoQuery(int UserId) : IQuery<OperationResult>;

public class GetUserInfoQueryHandler : IQueryHandler<GetUserInfoQuery, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public GetUserInfoQueryHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(GetUserInfoQuery query, CancellationToken cancellationToken)
    {
        var user = await _applicationDataContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == query.UserId, cancellationToken: cancellationToken);

        return user is null
            ? OperationResult.NotFound($"Пользователь с ID {query.UserId} не найден")
            : OperationResult.Ok(user);
    }
}