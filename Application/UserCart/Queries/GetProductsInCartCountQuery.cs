using Application.Common.Contracts;
using Domain.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.UserCart.Queries;

public record GetProductsInCartCountQuery(int UserId) : IQuery<OperationResult>;

public class GetProductsInCartCountQueryHandler : IQueryHandler<GetProductsInCartCountQuery, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public GetProductsInCartCountQueryHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(GetProductsInCartCountQuery query, CancellationToken cancellationToken)
    {
        var count = await _applicationDataContext.CartItems
            .AsNoTracking()
            .Where(x => x.UserId == query.UserId)
            .Select(x => x.Count)
            .SumAsync(cancellationToken: cancellationToken);
        
        return OperationResult.Ok(count);
    }
}