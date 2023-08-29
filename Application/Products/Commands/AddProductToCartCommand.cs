using Application.Common.Contracts;
using Domain.Common;
using Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands;

public record AddProductToCartData(int ProductId, int Count);

public record AddProductToCartCommand(int UserId, AddProductToCartData Data) : ICommand<OperationResult>;

public class AddProductToCartCommandHandler : ICommandHandler<AddProductToCartCommand, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public AddProductToCartCommandHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(AddProductToCartCommand command, CancellationToken cancellationToken)
    {
        var product = await _applicationDataContext.Products
            .SingleOrDefaultAsync(x => x.Id == command.Data.ProductId, cancellationToken: cancellationToken);

        if (product is null)
            return OperationResult.BadRequest($"Товар с ID {command.Data.ProductId} не найден");
        
        var cartItem = await _applicationDataContext.CartItems
            .Where(x => x.UserId == command.UserId && x.Product.Id == product.Id)
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (cartItem is not null)
        {
            cartItem.Count += command.Data.Count;

            _applicationDataContext.CartItems.Update(cartItem);
            await _applicationDataContext.SaveChangesAsync(cancellationToken);

            return OperationResult.Ok(cartItem);
        }

        cartItem = new CartItem
        {
            UserId = command.UserId,
            Product = product,
            Count = 1
        };

        var entry = await _applicationDataContext.CartItems.AddAsync(cartItem, cancellationToken);
        await _applicationDataContext.SaveChangesAsync(cancellationToken);

        return OperationResult.Ok(entry.Entity);
    }
}