namespace Application;

public record IdentityRequest
{
    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;
}