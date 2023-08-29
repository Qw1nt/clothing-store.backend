using Application.Common.Contracts;
using Domain.Common;
using Domain.Common.Configurations;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands;

public record UpdateProductImageCommand(int Id, IFormFile Image) : ICommand<OperationResult>;

public class UpdateProductImageCommandHandler : ICommandHandler<UpdateProductImageCommand, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;
    private readonly IFileSaveService _fileSaveService;

    public UpdateProductImageCommandHandler(IApplicationDataContext applicationDataContext,
        IFileSaveService fileSaveService)
    {
        _applicationDataContext = applicationDataContext;
        _fileSaveService = fileSaveService;
    }

    public async ValueTask<OperationResult> Handle(UpdateProductImageCommand command,
        CancellationToken cancellationToken)
    {
        var product = await _applicationDataContext.Products
            .SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

        if (product is null)
            return OperationResult.BadRequest();

        product.ImageUrl = await _fileSaveService.SaveAsync(command.Image, SavePaths.ProductsImages);

        var entry = _applicationDataContext.Products.Update(product);
        await _applicationDataContext.SaveChangesAsync(cancellationToken);
        return OperationResult.Ok(entry.Entity);
    }
}