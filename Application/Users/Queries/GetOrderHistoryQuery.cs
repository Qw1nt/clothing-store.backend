using System.Text.Json;
using Application.Common.Contracts;
using Domain.Common;
using Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public record GetOrderHistoryQuery(int UserId) : IQuery<OperationResult>;

public class GetOrderHistoryQueryHandler : IQueryHandler<GetOrderHistoryQuery, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public GetOrderHistoryQueryHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(GetOrderHistoryQuery query, CancellationToken cancellationToken)
    {
        var orders = await _applicationDataContext.Orders
            .AsNoTracking()
            .Where(x => x.UserId == query.UserId)
            .ToListAsync(cancellationToken: cancellationToken);

        var result = new List<OrderInHistory>();

        foreach (var order in orders)
        {
            List<CartItem> items = JsonSerializer.Deserialize<List<CartItem>>(order.JsonData) ?? new List<CartItem>();
            result.Add(new OrderInHistory {Items = items, Date = order.Date});
        }

        return OperationResult.Ok(result);
    }
}
