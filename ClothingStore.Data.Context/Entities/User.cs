using System.Text.Json.Serialization;

namespace ClothingStore.Data.Context.Entities;

public class User : Entity
{
    public string Login { get; set; } = null!;
    
    public string? FirstSecondNames { get; set; }
    
    public DateTime RegisterDate { get; set; }

    [JsonIgnore] public string PasswordHash { get; set; } = null!;

    [JsonIgnore] public string Salt { get; set; } = null!;

    public string Role { get; set; } = null!;
}