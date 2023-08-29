using Application.Common.Contracts;
using Domain.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.UserCatalog.Queries;

public record GetAllProductsQuery : IQuery<OperationResult>;

public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public GetAllProductsQueryHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public ValueTask<OperationResult> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = _applicationDataContext.Products
            .AsNoTracking();

        return ValueTask.FromResult(OperationResult.Ok(products));
    }
}