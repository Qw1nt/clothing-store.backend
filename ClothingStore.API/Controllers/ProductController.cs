using ClothingStore.Configurations;
using ClothingStore.Data.Context;
using ClothingStore.Data.Context.Entities;
using ClothingStore.Data.Requests;
using ClothingStore.Services.FileSaveService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.API.Controllers;

/// <summary>
/// Товары
/// </summary>
[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    private readonly DataContext _dataContext;
    private readonly FileSaveService _fileSaveService;
    
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="dataContext">Поставщик данных</param>
    /// <param name="fileSaveService"></param>
    public ProductController(DataContext dataContext, FileSaveService fileSaveService)
    {
        _dataContext = dataContext;
        _fileSaveService = fileSaveService;
    }

    /// <summary>
    /// Получить товар по ID
    /// </summary>
    /// <param name="productId">ID товара</param>
    /// <returns></returns>
    [HttpGet("{productId:int}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct([FromRoute] int productId)
    {
        var product = await _dataContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == productId);

        return product == null ? NotFound() : Ok(product);
    }

    /// <summary>
    /// Добавить товар
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description
        };

        await SaveProductImage(request.Image, product);

        var entry = await _dataContext.AddAsync(product);
        await _dataContext.SaveChangesAsync();
        
        return Ok(entry.Entity);
    }

    /// <summary>
    /// Удалить товар
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpDelete("{productId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct([FromRoute] int productId)
    {
        var product = await _dataContext.Products
            .FirstOrDefaultAsync(x => x.Id == productId);

        if (product is null)
            return NotFound();

        _dataContext.Products.Remove(product);
        await _dataContext.SaveChangesAsync();

        return Ok();
    }
    
    private async ValueTask SaveProductImage(IFormFile? formFile, Product product)
    {
        if (formFile is null)
            return;

        product.ImageUrl = await _fileSaveService.SaveAsync(formFile, SavePaths.ProductsImages);
    }

    private async Task SetProductCategories(List<int> ids, Product product)
    {
        var categories = await _dataContext.Categories
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();
        
        
    }
}