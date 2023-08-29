using Application.Common.Contracts;
using Domain.Common;
using Mediator;

namespace Application.UserCart.Commands;

public record UpdatePurchasedCountCommand(List<UpdateProductPurchasedCount> Products) : ICommand<OperationResult>;

public class UpdatePurchasedCountCommandHandler : ICommandHandler<UpdatePurchasedCountCommand, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public UpdatePurchasedCountCommandHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(UpdatePurchasedCountCommand command,
        CancellationToken cancellationToken)
    {
        var products = _applicationDataContext.Products
            .Where(x => command.Products.Any(u => u.ProductId == x.Id));

        foreach (var product in products)
        {
            var count = command.Products.First(x => x.ProductId == product.Id).Count;
            product.PurchasedCount += count;
        }

        _applicationDataContext.Products.UpdateRange(products);
        await _applicationDataContext.SaveChangesAsync(cancellationToken);
        return OperationResult.Ok();
    }
}