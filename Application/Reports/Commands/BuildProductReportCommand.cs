using System.Text.Json;
using Application.Common.Contracts;
using Domain.Common;
using Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Commands;

public record BuildProductReportCommand(List<int> ProductsIds, DateTime StartDate, DateTime EndDate) : ICommand<OperationResult>;

public class BuildProductReportCommandHandler : ICommandHandler<BuildProductReportCommand, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public BuildProductReportCommandHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(BuildProductReportCommand command, CancellationToken cancellationToken)
    {
        var products = await _applicationDataContext.Products
            .AsNoTracking()
            .Where(x => command.ProductsIds.Contains(x.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        var orders = await _applicationDataContext.Orders
            .AsNoTracking()
            .Where(x => x.Date > command.StartDate && x.Date < command.EndDate)
            .ToListAsync(cancellationToken: cancellationToken);

        var result = new List<ReportRecord>();
        
        foreach (var product in products)
        {
            var record = new ReportRecord {Product = product};

            foreach (var order in orders)
            {
                var data = JsonSerializer.Deserialize<List<CartItem>>(order.JsonData);

                var cartItem = data?.FirstOrDefault(x => x.Product.Id == product.Id);
                
                if (cartItem != null)
                    record.Count += cartItem.Count;
            }

            result.Add(record);
        }

        return OperationResult.Ok(result);
    }
}
