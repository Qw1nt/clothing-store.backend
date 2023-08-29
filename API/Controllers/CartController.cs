using API.Extensions;
using Application.Products.Commands;
using Application.UserCart.Commands;
using Application.UserCart.Queries;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Корзина
/// </summary>
[ApiController]
[Route("cart")]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Получить кол-во товаров в корзине
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("count")]
    public async Task<IActionResult> GetProductsInCartCount()
    {
        this.TryGetIdClaim(out int userId);
        var result = await _mediator.Send(new GetProductsInCartCountQuery(userId));
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Получить товары в корзине
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        this.TryGetIdClaim(out int userId);
        var result = await _mediator.Send(new GetProductsInCartQuery(userId));
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Оформить заказ
    /// </summary>
    /// <param name="commandData"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutCommandData commandData)
    {
        this.TryGetIdClaim(out int userId);
        var result = await _mediator.Send(new CheckoutCommand(userId, commandData));
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Обновить кол-во купленных товаров
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPut("purchased-count")]
    public async Task<IActionResult> UpdatePurchasedCount([FromBody] UpdateProductImageCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Удалить товар
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("item/{itemId:int}")]
    public async Task<IActionResult> DeleteItem([FromRoute] int itemId)
    {
        var result = await _mediator.Send(new RemoveProductFromCartProduct(itemId));
        return result.ToHttpResponse();
    }
}