using System.Text.Json;
using API.Extensions;
using Application.Users.Queries;
using Domain;
using Domain.Common;
using Domain.Entities;
using Mediator;
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
    private readonly IMediator _mediator;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
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
        var result = await _mediator.Send(new GetUserInfoQuery(id));
        return result.ToHttpResponse();
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
        var result = await _mediator.Send(new GetOrderHistoryQuery(userId));
        return result.ToHttpResponse();
    }
}