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