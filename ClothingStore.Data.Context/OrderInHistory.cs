using ClothingStore.Data.Context.Entities;

namespace ClothingStore.Data.Context;

public class OrderInHistory
{
    public List<CartItem> Items { get; set; } = new();
}