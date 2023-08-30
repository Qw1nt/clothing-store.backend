using API.Extensions;
using Application.UserIdentity.Commands;
using Domain.Entities;
using Domain.Common;
using Domain.Common.Configurations;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Контроллер идентификации
/// </summary>
[ApiController]
[Route("identity")]
public class IdentityController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="mediator"></param>
    public IdentityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(IdentityKeyPair), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Регистрация пользователя с ролью 
    /// </summary>
    /// <param name="role">Роль пользователя</param>
    /// <param name="command"></param>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpPost("register-with-role/{role}")]
    [ProducesResponseType(typeof(IdentityKeyPair), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterWithRole([FromRoute] string role, [FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(new RegisterWithRoleCommand(role, command));
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(IdentityKeyPair), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    /// <summary>
    /// Задать роль пользователю
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpPut("assign-role")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetRole([FromBody] SetRoleForUserCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }
}