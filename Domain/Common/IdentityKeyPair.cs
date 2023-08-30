namespace Domain.Common;

public record IdentityKeyPair(bool Success, string AccessToken, string? Error = null);
