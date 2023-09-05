using Application.Common.Contracts;
using Application.Common.Mapper;
using Application.Common.Validators;
using Domain.Common;
using Domain.Common.Configurations;
using Domain.Entities;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProductMapper = Application.Common.Mapper.ProductMapper;

namespace Application.Products.Commands;

public record AddProductCommand(
    List<int> CategoriesIds,
    string Name,
    string? Description,
    int Price,
    string Color,
    IFormFile? Image
) : ICommand<OperationResult>;

public class AddProductCommandHandler : ICommandHandler<AddProductCommand, OperationResult>
{
    private readonly IApplicationDataContext _applicationDataContext;
    private readonly IFileSaveService _fileSaveService;

    public AddProductCommandHandler(IApplicationDataContext applicationDataContext, IFileSaveService fileSaveService)
    {
        _applicationDataContext = applicationDataContext;
        _fileSaveService = fileSaveService;
    }

    public async ValueTask<OperationResult> Handle(AddProductCommand command, CancellationToken cancellationToken)
    {
        var validationResult =
            await FluentModelValidator.ExecuteAsync<CreateProductRequestValidator, AddProductCommand>(command);
        if (validationResult.IsValid == false)
            return OperationResult.BadRequest(validationResult.Errors);

        var product = command.ToProduct();

        if (command.Image != null)
            product.ImageUrl = await _fileSaveService.SaveAsync(command.Image, SavePaths.ProductsImages);

        await SetProductCategories(command.CategoriesIds, product);

        var entry = await _applicationDataContext.Products.AddAsync(product, cancellationToken);
        await _applicationDataContext.SaveChangesAsync(cancellationToken);

        return OperationResult.Ok(entry.Entity);
    }
    
    private async Task SetProductCategories(List<int> ids, Product product)
    {
        var categories= await _applicationDataContext.Categories
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();

        var existCategories = await _applicationDataContext.Categories
            .Where(x => x.Products.Contains(product))
            .ToListAsync();

        foreach (var category in existCategories)
            category.Products.Remove(product);
        
        _applicationDataContext.Categories.UpdateRange(existCategories);

        foreach (var category in categories)
            category.Products.Add(product);

        _applicationDataContext.Categories.UpdateRange(categories);
    }
}