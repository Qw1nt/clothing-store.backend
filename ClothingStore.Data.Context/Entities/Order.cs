namespace ClothingStore.Data.Context.Entities;

public class Order : Entity
{
    public int UserId { get; set; }
    
    public string JsonData { get; set; } = null!;
}