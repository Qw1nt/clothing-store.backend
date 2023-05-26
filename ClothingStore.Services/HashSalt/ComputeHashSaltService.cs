using System.Security.Cryptography;

namespace ClothingStore.Services.HashSalt;

public class ComputeHashSaltService : IHashSaltService
{
    private const int DefaultHashIterations = 10101;
    private readonly int _computingHashIterations;

    public ComputeHashSaltService()
    {
        _computingHashIterations = DefaultHashIterations;
    }
    
    public ComputeHashSaltService(int computingHashIterations)
    {
        _computingHashIterations = computingHashIterations;
    }

    public string Salt()
    {
        var randomNumberGenerator = RandomNumberGenerator.Create();
        Span<byte> salt = stackalloc byte[24];
        randomNumberGenerator.GetBytes(salt);
        return Convert.ToBase64String(salt);
    }
    
    public string Hash(string sourceValue, string forSalt)
    {
        var salt = Convert.FromBase64String(forSalt);

        using var hashGenerator = new Rfc2898DeriveBytes(sourceValue, salt, _computingHashIterations, HashAlgorithmName.SHA256);
        var bytes = hashGenerator.GetBytes(24);

        return Convert.ToBase64String(bytes);
    }
}