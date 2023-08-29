using Application.Common.Contracts;
using Application.Products.Queries;
using Domain.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.UserCatalog.Queries;

public record GetProductsByCategoryQuery(int CategoryId) : IQuery<OperationResult>;

public class GetProductsByCategoryQueryHandler : IQueryHandler<GetProductsByCategoryQuery, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public GetProductsByCategoryQueryHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public ValueTask<OperationResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var result = _applicationDataContext.Categories
            .AsNoTracking()
            .Where(x => x.Id == query.CategoryId)
            .Select(x => x.Products);

        return ValueTask.FromResult(OperationResult.Ok(result));
    }
}