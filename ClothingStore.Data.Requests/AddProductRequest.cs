using Microsoft.AspNetCore.Http;

namespace ClothingStore.Data.Requests;

public record AddProductRequest
{
    public List<int> CategoriesIds { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    
    public string? Description { get; set; }

    public int Price { get; set; }

    public string Color { get; set; } = null!;
    
    public IFormFile? Image { get; set; }
}