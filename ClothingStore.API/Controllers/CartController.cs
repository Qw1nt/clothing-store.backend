using System.Text.Json;
using ClothingStore.API.Extensions;
using ClothingStore.Data.Context;
using ClothingStore.Data.Context.Entities;
using ClothingStore.Data.Requests;
using ClothingStore.Data.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.API.Controllers;

/// <summary>
/// Корзина
/// </summary>
[ApiController]
[Route("cart")]
public class CartController : ControllerBase
{
    private readonly DataContext _dataContext;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="dataContext"></param>
    public CartController(DataContext dataContext)
    {
        _dataContext = dataContext;
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
        
        var count = await _dataContext.CartItems
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => x.Count)
            .SumAsync();
            
        return Ok(count);
    }

    /// <summary>
    /// Получить товары в корзине
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public IActionResult GetCart()
    {
        this.TryGetIdClaim(out int id);

        var result = _dataContext.CartItems
            .AsNoTracking()
            .Where(x => x.UserId == id)
            .Include(x => x.Product);

        return Ok(result);
    }
    
    /// <summary>
    /// Добавить товар в корзину
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCartRequest request)
    {
        var product = await _dataContext.Products
            .FirstOrDefaultAsync(x => x.Id == request.ProductId);

        if (product is null)
            return BadRequest($"Товар с ID {request.ProductId} не найден");
        
        this.TryGetIdClaim(out int userId);

        var cartItem = await _dataContext.CartItems
            .Where(x => x.UserId == userId && x.Product.Id == product.Id)
            .SingleOrDefaultAsync();

        if (cartItem is not null)
        {
            cartItem.Count += request.Count;
            
            _dataContext.CartItems.Update(cartItem);
            await _dataContext.SaveChangesAsync();
            
            return Ok(cartItem);
        }

        cartItem = new CartItem
        {
            UserId = userId,
            Product = product,
            Count = 1
        };

        await _dataContext.CartItems.AddAsync(cartItem);
        await _dataContext.SaveChangesAsync();
        
        return Ok();
    }

    /// <summary>
    /// Оформить заказ
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
    {
        var validationResult =
            await FluentModelValidator.ExecuteAsync<CheckoutRequestValidator, CheckoutRequest>(request);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.Errors);
        
        this.TryGetIdClaim(out int id);

        var user = await _dataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
            return BadRequest("Пользователь не найден");
        
        var orderJsonData = JsonSerializer.Serialize(request.CartItems);
        await _dataContext.Orders.AddAsync(new Order()
        {
            UserId = user.Id,
            JsonData = orderJsonData
        });
        
        _dataContext.CartItems.RemoveRange(request.CartItems);
        
        await _dataContext.SaveChangesAsync();
        return Ok(request.CartItems);
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
        var item = await _dataContext.CartItems
            .FirstOrDefaultAsync(x => x.Id == itemId);

        if (item is null)
            return NotFound($"Не удалось найти товар с ID {itemId}");
        
        _dataContext.CartItems.Remove(item);
        await _dataContext.SaveChangesAsync();

        return Ok();
    }
}