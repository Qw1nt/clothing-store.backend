namespace ClothingStore.Data.Requests;

public class IdentityRequest
{
    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;
}