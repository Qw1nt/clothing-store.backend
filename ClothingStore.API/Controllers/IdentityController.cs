using System.Security.Claims;
using ClothingStore.Configurations;
using ClothingStore.Data.Context;
using ClothingStore.Data.Context.Entities;
using ClothingStore.Data.Requests;
using ClothingStore.Data.Responses;
using ClothingStore.Services;
using ClothingStore.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ClothingStore.API.Controllers;

/// <summary>
/// Контроллер идентификации
/// </summary>
[ApiController]
[Route("identity")]
public class IdentityController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;
    private readonly IAuthenticationService _authenticationService;
    private readonly DataContext _dataContext;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="memoryCache"></param>
    /// <param name="authenticationService"></param>
    /// <param name="dataContext"></param>
    public IdentityController(IMemoryCache memoryCache, IAuthenticationService authenticationService, DataContext dataContext)
    {
        _memoryCache = memoryCache;
        _authenticationService = authenticationService;
        _dataContext = dataContext;
    }

    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        return await RegisterInternal(request);
    }

    /// <summary>
    /// Регистрация пользователя с ролью 
    /// </summary>
    /// <param name="role">Роль пользователя</param>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpPost("register-with-role/{role}")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterWithRole(string role, [FromBody] RegisterRequest request)
    {
        return await RegisterInternal(request, role);
    }

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] IdentityRequest request)
    {
        var result = await _authenticationService.Login(request);

        return result.Success ? Ok(result) : NotFound(result.Error);
    }

    /// <summary>
    /// Задать роль пользователю
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize(Policy = IdentityConfiguration.Policy.Admin)]
    [HttpPut("assign-role")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetRole([FromBody] AssignRoleToUserRequest request)
    {
        var user = await _dataContext.Users
            .FirstOrDefaultAsync(x => x.Id == request.UserId);

        if (user == null)
            return NotFound();

        user.Role = request.Role;
        _dataContext.Users.Update(user);
        await _dataContext.SaveChangesAsync();

        return Ok(user);
    }

    private async Task<IActionResult> RegisterInternal(RegisterRequest request, string? role = null)
    {
        var canRegister = await PossibilityRegistration(request);
        if (canRegister.StatusCode != StatusCodes.Status200OK)
            return canRegister;
        
        _memoryCache.Set(request.Login, request);
        var result = await _authenticationService.Register(request, role);
        _memoryCache.Remove(request.Login);
        
        return result.Success ? Ok() : BadRequest(result.Error);
    }
    
    private async Task<ObjectResult> PossibilityRegistration(RegisterRequest request)
    {
        if (_memoryCache.TryGetValue(request.Login, out object? value) == true)
            return BadRequest("Error");

        if (await _dataContext.Users.AnyAsync(x => x.Login == request.Login))
            return BadRequest("Пользователь с таким логином уже зарегистрирован");

        return Ok("Success");
    }
}