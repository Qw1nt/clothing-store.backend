using System.Text.Json;
using API.Extensions;
using Domain;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly DataContext _dataContext;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="dataContext"></param>
    public UserController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <summary>
    /// Получить информацию
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInfo()
    {
        this.TryGetIdClaim(out int id);

        var user = await _dataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user is null)
            return NotFound($"Пользователь с ID {id} не найден");

        return Ok(user);
    }

    /// <summary>
    /// Получить историю покупок
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("order/history")]
    [ProducesResponseType(typeof(List<OrderInHistory>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderHistory()
    {
        this.TryGetIdClaim(out int userId);

        var orders = await _dataContext.Orders
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync();

        var result = new List<OrderInHistory>();

        foreach (var order in orders)
        {
            List<CartItem> items = JsonSerializer.Deserialize<List<CartItem>>(order.JsonData) ?? new();

            result.Add(new OrderInHistory {Items = items, Date = order.Date});
        }

        return Ok(result);
    }
}