using System.Text.Json;
using Application.Common.Contracts;
using Application.Common.Validators;
using Domain.Common;
using Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.UserCart.Commands;

public record CheckoutCommandData(string Name, string PhoneNumber, string Email, string? Comment, string Address, List<CartItem> CartItems);

public record CheckoutCommand(int UserId, CheckoutCommandData Data) : ICommand<OperationResult>;

public class CheckoutCommandHandler : ICommandHandler<CheckoutCommand, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public CheckoutCommandHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(CheckoutCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await FluentModelValidator.ExecuteAsync<CheckoutRequestValidator, CheckoutCommandData>(command.Data);
        if (validationResult.IsValid == false)
            return OperationResult.BadRequest(validationResult.Errors);
        
        var user = await _applicationDataContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == command.UserId, cancellationToken: cancellationToken);

        if (user is null)
            return OperationResult.BadRequest("Пользователь не найден");

        var orderJsonData = JsonSerializer.Serialize(command.Data.CartItems);
        await _applicationDataContext.Orders.AddAsync(new Order
        {
            UserId = user.Id,
            JsonData = orderJsonData,
            Date = DateTime.UtcNow
        }, cancellationToken);

        _applicationDataContext.CartItems.RemoveRange(command.Data.CartItems);
        await _applicationDataContext.SaveChangesAsync(cancellationToken);

        return OperationResult.Ok(command.Data.CartItems);
    }
}