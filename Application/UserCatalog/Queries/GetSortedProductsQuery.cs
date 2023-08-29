using Application.Common.Contracts;
using Domain.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.UserCatalog.Queries;

public record GetSortedProductsQuery : IQuery<OperationResult>;

public class GetSortedProductsQueryHandler : IQueryHandler<GetSortedProductsQuery, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public GetSortedProductsQueryHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public ValueTask<OperationResult> Handle(GetSortedProductsQuery query, CancellationToken cancellationToken)
    {
        var orderByDescending = _applicationDataContext.Products
            .AsNoTracking()
            .OrderByDescending(x => x.Id);

        return ValueTask.FromResult(OperationResult.Ok(orderByDescending));
    }
}
