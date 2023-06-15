using ClothingStore.API.Extensions;
using ClothingStore.Configurations;
using ClothingStore.Data.Context;
using ClothingStore.Data.Context.Entities;
using ClothingStore.Data.Requests;
using ClothingStore.Data.Validators;
using ClothingStore.Services.FileSaveService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ClothingStore.API.Controllers;

/// <summary>
/// Товары
/// </summary>
[ApiController]
[Route("product")]
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
            .Include(x => x.Reviews)!
                .ThenInclude(x => x.Owner)
            .FirstOrDefaultAsync(x => x.Id == productId);

        return product == null ? NotFound() : Ok(product);
    }

    /// <summary>
    /// Добавить отзыв на товар
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("review")]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewForProductRequest request)
    {
        this.TryGetIdClaim(out int userId);

        var alreadyCreate = _dataContext.Products
            .AsNoTracking()
            .Any(x => x.Reviews != null && x.Reviews.Any(y => y.Owner.Id == userId));

        if (alreadyCreate == true)
            return BadRequest("Вы уже оставляли отзыв на этот товар");

        var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null)
            return BadRequest();
        
        var review = new Review
        {
            Owner = user,
            Title = request.Title,
            Content = request.Content,
            Date = DateTime.UtcNow
        };

        var reviewEntry = await _dataContext.Reviews.AddAsync(review);

        var product = await _dataContext.Products
            .FirstOrDefaultAsync(x => x.Id == request.ProductId);
        if (product is null)
            return BadRequest();

        product.Reviews ??= new List<Review>();
        product.Reviews.Add(reviewEntry.Entity);
        
        await _dataContext.SaveChangesAsync();
        return Ok(reviewEntry.Entity);
    }

    /// <summary>
    /// Добавить товар
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Manager)]
    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromForm] AddProductRequest request)
    {
        var validationResult = await FluentModelValidator.ExecuteAsync<CreateProductRequestValidator, AddProductRequest>(request);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.Errors);

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };

        await SetProductImage(request.Image, product);
        await SetProductCategories(request.CategoriesIds, product);

        var entry = await _dataContext.AddAsync(product);
        await _dataContext.SaveChangesAsync();

        return Ok(entry.Entity);
    }

    /// <summary>
    /// Обновить изображение
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    [HttpPut("image/{productId:int}")]
    public async Task<IActionResult> UpdateProductImage([FromRoute] int productId, [FromForm] IFormFile image)
    {
        var product = await _dataContext.Products
            .FirstOrDefaultAsync(x => x.Id == productId);

        if (product is null)
            return BadRequest();
        
        await SetProductImage(image, product);
        _dataContext.Products.Update(product);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Изменить информацию о товаре
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = IdentityConfiguration.Policy.Manager)]
    [HttpPut("{productId:int}")]
    public async Task<IActionResult> EditProduct([FromRoute] int productId, [FromBody] EditProductRequest request)
    {
        var product = await _dataContext.Products
            .Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.Id == productId);

        if (product is null)
            return NotFound();

        await EditProduct(product, request);

        _dataContext.Products.Update(product);
        await _dataContext.SaveChangesAsync();

        return Ok(product);
    }

    /// <summary>
    /// Удалить товар
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Manager)]
    [HttpDelete("{productId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct([FromRoute] int productId)
    {
        var product = await _dataContext.Products
            .FirstOrDefaultAsync(x => x.Id == productId);

        if (product is null)
            return NotFound();
        
        // product.Categories = null;
        await _dataContext.SaveChangesAsync();
        
        _dataContext.Products.Remove(product);
        await _dataContext.SaveChangesAsync();

        return Ok();
    }

    private async ValueTask SetProductImage(IFormFile? formFile, Product product)
    {
        if (formFile is null)
            return;

        product.ImageUrl = await _fileSaveService.SaveAsync(formFile, SavePaths.ProductsImages);
    }

    private async Task SetProductCategories(List<int> ids, Product product)
    {
        var categories= await _dataContext.Categories
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();

        var existCategories = await _dataContext.Categories
            .Where(x => x.Products.Contains(product))
            .ToListAsync();

        foreach (var category in existCategories)
        {
            category.Products.Remove(product);
        }
        
        _dataContext.Categories.UpdateRange(existCategories);

        foreach (var category in categories)
        {
            category.Products.Add(product);
        }

        _dataContext.Categories.UpdateRange(categories);
    }

    private async ValueTask EditProduct(Product product, EditProductRequest request)
    {
        if (request.Categories is not null)
            await SetProductCategories(request.Categories, product);
        
        if (request.Name is not null)
            product.Name = request.Name;

        if (request.Description is not null)
            product.Description = request.Description;

        if (request.Price is not null)
            product.Price = (double) request.Price;
    }
}