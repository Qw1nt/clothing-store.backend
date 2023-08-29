using Application.Common.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.UpdateProductService;

public class UpdateProductService : IUpdateProductService
{
    private readonly IApplicationDataContext _applicationDataContext;

    public UpdateProductService(IApplicationDataContext applicationDataContext)
    {
        _applicationDataContext = applicationDataContext;
    }

    public async Task SetProductCategories(List<int> ids, Product product)
    {
        var categories= await _applicationDataContext.Categories
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();

        var existCategories = await _applicationDataContext.Categories
            .Where(x => x.Products.Contains(product))
            .ToListAsync();

        foreach (var category in existCategories)
        {
            category.Products.Remove(product);
        }
        
        _applicationDataContext.Categories.UpdateRange(existCategories);

        foreach (var category in categories)
        {
            category.Products.Add(product);
        }

        _applicationDataContext.Categories.UpdateRange(categories);
    }
}