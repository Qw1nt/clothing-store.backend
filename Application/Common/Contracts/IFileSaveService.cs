using Microsoft.AspNetCore.Http;

namespace Application.Common.Contracts;

public interface IFileSaveService
{
    Task<string> SaveAsync(IFormFile formFile, string saveFolder);
}