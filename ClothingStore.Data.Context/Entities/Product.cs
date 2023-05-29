namespace ClothingStore.Data.Context.Entities;

public class Product : Entity
{
    public List<Category>? Categories { get; set; } = new();

    public string Name { get; set; } = null!;

    public string? Description { get; set; } = null!;
    
    public double Price { get; set; }

    public int PurchasedCount { get; set; }

    public string? Color { get; set; }
    
    public string? ImageUrl { get; set; }

    public List<Review>? Reviews { get; set; } = new();
}