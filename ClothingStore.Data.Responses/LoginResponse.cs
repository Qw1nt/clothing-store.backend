namespace ClothingStore.Data.Responses;

public class LoginResponse
{
    public bool Success { get; set; }
    
    public string AccessToken { get; set; } = null!;
    public string? Error { get; set; } 
}