using ClothingStore.Data.Context.Entities;

namespace ClothingStore.Data.Context;

public class ReportRecord
{
    public Product Product { get; set; } = null!;
    
    public int Count { get; set; }
}