using Microsoft.AspNetCore.Http;

namespace ClothingStore.Services.FileSaveService;

public interface IFileSaveService<T>
{
    Task<T> SaveAsync(IFormFile formFile, string saveFolder);
}