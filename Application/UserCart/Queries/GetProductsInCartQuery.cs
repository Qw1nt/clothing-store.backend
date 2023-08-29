using Application.Common.Contracts;
using Domain.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.UserCart.Queries;

public record GetProductsInCartQuery(int UserId) : IQuery<OperationResult>;

public class GetProductsInCartQueryHandler : IQueryHandler<GetProductsInCartQuery, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public GetProductsInCartQueryHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(GetProductsInCartQuery query, CancellationToken cancellationToken)
    {
        var result = _applicationDataContext.CartItems
            .AsNoTracking()
            .Where(x => x.UserId == query.UserId)
            .Include(x => x.Product);

        return await ValueTask.FromResult(OperationResult.Ok(result));
    }
}