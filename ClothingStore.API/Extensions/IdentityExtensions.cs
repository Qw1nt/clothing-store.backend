using ClothingStore.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.API.Extensions;

/// <summary>
/// 
/// </summary>
public static class IdentityExtensions 
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerBase"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool TryGetIdClaim(this ControllerBase controllerBase, out int id)
    {
        id = -1;
        
        var item = controllerBase.HttpContext.Items[IdentityConfiguration.IdClaim];
        if (item is null)
            return false;
            
        int.TryParse(item.ToString(), null, out id);
        return true;
    }
}

/// <summary>
/// 
/// </summary>
public class ClaimsMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="next"></param>
    public ClaimsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Вызывается автоматически
    /// </summary>
    /// <param name="httpContext"></param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.User.Identity is not null && httpContext.User.Identity.IsAuthenticated)
        {
            var userId = int.Parse(httpContext.User.FindFirst(IdentityConfiguration.IdClaim)!.Value);
            httpContext.Items[IdentityConfiguration.IdClaim] = userId;
        }

        await _next(httpContext);
    }
}

/// <summary>
/// 
/// </summary>
public static class ClaimsMiddlewareExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="application"></param>
    /// <returns></returns>
    public static WebApplication UseClaimsDetermination(this WebApplication application)
    {
        application.UseMiddleware<ClaimsMiddleware>();
        return application;
    }
}