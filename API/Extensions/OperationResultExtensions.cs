using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions;

/// <summary>
/// 
/// </summary>
public static class OperationResultExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="operationResult"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ObjectResult ToHttpResponse(this OperationResult operationResult)
    {
        return new ObjectResult(operationResult.Data)
        {
            StatusCode = operationResult.StatusCode,
        };
    }
}