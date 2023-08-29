using Application.Common.Contracts;
using Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Queries;

public record GetProductQuery(int Id) : IQuery<Product?>;

public class GetProductQueryHandler : IQueryHandler<GetProductQuery, Product?>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public GetProductQueryHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<Product?> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var result = await _applicationDataContext.Products
            .AsNoTracking()
            .Include(x => x.Reviews)!
            .ThenInclude(x => x.Owner)
            .SingleOrDefaultAsync(x => x.Id == query.Id, cancellationToken: cancellationToken);

        return result;
    }
}