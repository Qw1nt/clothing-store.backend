namespace ClothingStore.Data.Context.Entities;

public class CartItem : Entity
{
    public int UserId { get; set; }
    
    public Product Product { get; set; } = null!;
    
    public int Count { get; set; }
}