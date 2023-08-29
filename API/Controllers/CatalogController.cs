using API.Extensions;
using Application.UserCatalog.Queries;
using Domain;
using Domain.Common.Configurations;
using Domain.Entities;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Каталог товаров
/// </summary>
[ApiController]
[Route("catalog")]
public class CatalogController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Получить список всех товаров
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetAllProductsQuery());
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Получить 3 последних добавленных товара
    /// </summary>
    /// <returns></returns>
    [HttpGet("top-new/{count}")]
    [ProducesResponseType(typeof(IQueryable<Product>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNewProducts([FromRoute] int count)
    {
        var result = await _mediator.Send(new GetTopNewProductsQuery(count));
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Получить хиты продаж
    /// </summary>
    /// <returns></returns>
    [HttpGet("bestsellers")]
    public async Task<IActionResult> GetBestsellers()
    {
        var result = await _mediator.Send(new GetBestsellersQuery());
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Получить товары в категории
    /// </summary>
    /// <param name="categoryId">ID категории</param>
    /// <returns></returns>
    [HttpGet("category/{categoryId:int}")]
    public async Task<IActionResult> GetByCategory([FromRoute] int categoryId)
    {
        var result = await _mediator.Send(new GetProductsByCategoryQuery(categoryId));
        return result.ToHttpResponse();
    }
    
    /// <summary>
    /// Получить отсортированный список товаров
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpGet("sorted")]
    public async Task<IActionResult> GetSorted()
    {
        var result = await _mediator.Send(new GetSortedProductsQuery());
        return result.ToHttpResponse();
    }
}