using Application.Common.Contracts;
using Domain.Common;
using Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands;

public record EditProductCommand(int Id, string? Name, string? Description, double? Price, string? Color,
    List<int>? Categories) : ICommand<OperationResult>;

public class EditProductCommandHandler : ICommandHandler<EditProductCommand, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;
    private readonly IUpdateProductService _updateProductService;

    public EditProductCommandHandler(IApplicationDataContext applicationDataContext, IUpdateProductService updateProductService)
    {
        _applicationDataContext = applicationDataContext;
        _updateProductService = updateProductService;
    }

    public async ValueTask<OperationResult> Handle(EditProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _applicationDataContext.Products
            .Include(x => x.Reviews)
            .SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

        if (product is null)
            return OperationResult.NotFound();

        await EditProduct(product, command);

        var entry = _applicationDataContext.Products.Update(product);
        await _applicationDataContext.SaveChangesAsync(cancellationToken);
        
        return OperationResult.Ok(entry.Entity);
    }

    private async ValueTask EditProduct(Product product, EditProductCommand command)
    {
        if (command.Categories is not null)
            await _updateProductService.SetProductCategories(command.Categories, product);

        if (command.Name is not null)
            product.Name = command.Name;

        if (command.Description is not null)
            product.Description = command.Description;

        if (command.Price is not null)
            product.Price = (double) command.Price;
    }
}