using Application.Common.Contracts;
using Domain.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.UserCatalog.Queries;

public record GetBestsellersQuery : IQuery<OperationResult>;

public class GetBestsellersQueryHandler : IQueryHandler<GetBestsellersQuery, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public GetBestsellersQueryHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public ValueTask<OperationResult> Handle(GetBestsellersQuery query, CancellationToken cancellationToken)
    {
        var result = _applicationDataContext.Products
            .AsNoTracking()
            .OrderByDescending(p => p.PurchasedCount)
            .Take(8);

        return ValueTask.FromResult(OperationResult.Ok(result));
    }
}