using Application.Common.Contracts;
using Domain.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.UserCart.Commands;

public record RemoveProductFromCartProduct(int ProductId) : ICommand<OperationResult>;

public class RemoveProductFromCartProductHandler : ICommandHandler<RemoveProductFromCartProduct, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public RemoveProductFromCartProductHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(RemoveProductFromCartProduct command, CancellationToken cancellationToken)
    {
        var item = await _applicationDataContext.CartItems
            .SingleOrDefaultAsync(x => x.Id == command.ProductId, cancellationToken: cancellationToken);

        if (item is null)
            return OperationResult.NotFound($"Не удалось найти товар с ID {command.ProductId}");

        _applicationDataContext.CartItems.Remove(item);
        await _applicationDataContext.SaveChangesAsync(cancellationToken);
        return OperationResult.Ok();
    }
}