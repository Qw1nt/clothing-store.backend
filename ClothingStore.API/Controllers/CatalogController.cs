using ClothingStore.Configurations;
using ClothingStore.Data.Context;
using ClothingStore.Data.Context.Entities;
using Microsoft.AspNetCore.Authorization;
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
        var products = _dataContext.Products
            .AsNoTracking();

        return Ok(products);
    }

    /// <summary>
    /// Получить 3 последних добавленных товара
    /// </summary>
    /// <returns></returns>
    [HttpGet("top-new/{count}")]
    [ProducesResponseType(typeof(IQueryable<Product>), StatusCodes.Status200OK)]
    public IActionResult GetNewProducts([FromRoute] int count)
    {
        var result = _dataContext.Products
            .AsNoTracking()
            .OrderByDescending(p => p.Id)
            .Take(count);

        return Ok(result);
    }

    /// <summary>
    /// Получить хиты продаж
    /// </summary>
    /// <returns></returns>
    [HttpGet("bestsellers")]
    public IActionResult GetBestsellers()
    {
        var result = _dataContext.Products
            .AsNoTracking()
            .OrderByDescending(p => p.PurchasedCount)
            .Take(8);

        return Ok(result);
    }

    /// <summary>
    /// Получить товары в категории
    /// </summary>
    /// <param name="categoryId">ID категории</param>
    /// <returns></returns>
    [HttpGet("category/{categoryId:int}")]
    public IActionResult GetByCategory([FromRoute] int categoryId)
    {
        var result = _dataContext.Categories
            .AsNoTracking()
            .Where(x => x.Id == categoryId)
            .Select(x => x.Products);

        return Ok(result);
    }

    /// <summary>
    /// Получить отсортированный список товаров
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpGet("sorted")]
    public IActionResult GetSorted()
    {
        var orderByDescending = _dataContext.Products
            .AsNoTracking()
            .OrderByDescending(x => x.Id);

        return Ok(orderByDescending);
    }
}