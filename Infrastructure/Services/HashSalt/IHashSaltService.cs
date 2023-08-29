namespace Infrastructure.Services.HashSalt;

public interface IHashSaltService
{
    string Salt();

    string Hash(string sourceValue, string forSalt);
}