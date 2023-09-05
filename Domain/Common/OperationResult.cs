using Microsoft.AspNetCore.Http;

namespace Domain.Common;

public class OperationResult
{
        public int StatusCode { get; init; }

        public bool IsSuccessfulResult { get; init; }
     
        public object? Data { get; init; }

        private static OperationResult Create(int statusCode, bool isSuccessful, object? data = null)
        {
            return new OperationResult
            {
                StatusCode = statusCode,
                IsSuccessfulResult = isSuccessful,
                Data = data
            };
        }

        public static OperationResult Ok(object? data = null)
        {
            return Create(StatusCodes.Status200OK, true, data);
        }
        
        public static OperationResult BadRequest(object? data = null)
        {
            return Create(StatusCodes.Status400BadRequest, false, data);
        }   
        
        public static OperationResult NotFound(object? data = null)
        {
            return Create(StatusCodes.Status404NotFound, false, data);
        }
}