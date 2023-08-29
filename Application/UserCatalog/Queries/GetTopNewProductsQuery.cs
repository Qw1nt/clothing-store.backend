using Application.Common.Contracts;
using Domain.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.UserCatalog.Queries;

public record GetTopNewProductsQuery(int Count) : IQuery<OperationResult>;

public class GetTopNewProductsQueryHandler : IQueryHandler<GetTopNewProductsQuery, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public GetTopNewProductsQueryHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public ValueTask<OperationResult> Handle(GetTopNewProductsQuery query, CancellationToken cancellationToken)
    {
        var result = _applicationDataContext.Products
            .AsNoTracking()
            .OrderByDescending(p => p.Id)
            .Take(query.Count);

        return ValueTask.FromResult(OperationResult.Ok(result));
    }
}