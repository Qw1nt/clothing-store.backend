using API.Extensions;
using Application.Products.Commands;
using Application.Products.Queries;
using Application.Reviews.Commands;
using Domain.Entities;
using Domain.Common.Configurations;
using Infrastructure.Services.FileSaveService;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Товары
/// </summary>
[ApiController]
[Route("product")]
public class ProductController : ControllerBase
{
    private readonly FileSaveService _fileSaveService;
    private readonly IMediator _mediator;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="fileSaveService"></param>
    public ProductController(FileSaveService fileSaveService, IMediator mediator)
    {
        _fileSaveService = fileSaveService;
        _mediator = mediator;
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
        var product = await _mediator.Send(new GetProductQuery(productId));
        return product == null ? NotFound() : Ok(product);
    }

    /// <summary>
    /// Добавить товар в корзину
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCartData data)
    {
        this.TryGetIdClaim(out int userId);
        var result = await _mediator.Send(new AddProductToCartCommand(userId, data));
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Добавить отзыв на товар
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("review")]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewCommand command)
    {
        this.TryGetIdClaim(out int userId);
        var result = await _mediator.Send(new CreateReviewForProductCommand(userId, command));
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Добавить товар
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromForm] AddProductCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
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
        var result = await _mediator.Send(new UpdateProductImageCommand(productId, image));
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Изменить информацию о товаре
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpPut("{productId:int}")]
    public async Task<IActionResult> EditProduct([FromBody] EditProductCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
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
        var result = await _mediator.Send(new DeleteProductCommand(productId));
        return result.ToHttpResponse();
    }
}