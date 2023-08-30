using Application.Common.Contracts;
using Domain.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Queries;

public record GetAllCategories : IQuery<OperationResult>;

public class GetAllCategoriesHandler : IQueryHandler<GetAllCategories, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public GetAllCategoriesHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public ValueTask<OperationResult> Handle(GetAllCategories categories, CancellationToken cancellationToken)
    {
        var result = _applicationDataContext.Categories
            .AsNoTracking();

        return ValueTask.FromResult(OperationResult.Ok(result));
    }
}