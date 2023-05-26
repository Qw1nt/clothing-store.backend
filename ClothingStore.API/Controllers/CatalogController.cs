using ClothingStore.Data.Context;
using ClothingStore.Data.Context.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.API.Controllers;

/// <summary>
/// Каталог товаров
/// </summary>
[ApiController]
[Route("catalog")]
public class CatalogController : ControllerBase
{
    private readonly DataContext _dataContext;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="dataContext"></param>
    public CatalogController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <summary>
    /// Получить список всех товаров
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        var products =  _dataContext.Products
            .AsNoTracking();

        return Ok(products);
    }

    /// <summary>
    /// Получить товар по ID
    /// </summary>
    /// <param name="productId">ID товара</param>
    /// <returns></returns>
    [HttpGet("{productId:int}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] int productId)
    {
        var product = await _dataContext.Products
            .AsNoTracking()
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == productId);

        if (product is null)
            return NotFound();
        
        return Ok(product);
    }
}