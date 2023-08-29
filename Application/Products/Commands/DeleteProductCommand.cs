using Application.Common.Contracts;
using Domain.Common;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands;

public record DeleteProductCommand(int ProductId) : ICommand<OperationResult>;

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;

    public DeleteProductCommandHandler(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async ValueTask<OperationResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _applicationDataContext.Products
            .SingleOrDefaultAsync(x => x.Id == command.ProductId, cancellationToken: cancellationToken);

        if (product is null)
            return OperationResult.NotFound();
        
        await _applicationDataContext.SaveChangesAsync(cancellationToken);
        
        _applicationDataContext.Products.Remove(product);
        await _applicationDataContext.SaveChangesAsync(cancellationToken);

        return OperationResult.Ok();
    }
}