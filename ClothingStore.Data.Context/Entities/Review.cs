namespace ClothingStore.Data.Context.Entities;

public class Review : Entity
{
    public User Owner { get; set; } = null!;

    public string Title { get; set; } = null!;
    
    public int Rating { get; set; }
    
    public string Content { get; set; } = null!;
    
    public List<string>? Pluses { get; set; }
    
    public List<string>? Minuses { get; set; }
}