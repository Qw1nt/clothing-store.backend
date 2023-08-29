using Domain.Common;

namespace Domain.Entities;

public class Review : Entity
{
    public User Owner { get; set; } = null!;

    public string Title { get; set; } = null!;
    
    public string Content { get; set; } = null!;
    
    public DateTime Date { get; set; }
}