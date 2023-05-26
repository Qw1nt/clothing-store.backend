using Microsoft.AspNetCore.Http;

namespace ClothingStore.Data.Requests;

public record AddProductRequest
{
    public string Name { get; set; } = null!;

    public List<int> CategoriesIds { get; set; } = null!;

    public string? Description { get; set; }

    public IFormFile? Image { get; set; }
}