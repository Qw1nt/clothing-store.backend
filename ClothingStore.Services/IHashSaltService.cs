namespace ClothingStore.Services;

public interface IHashSaltService
{
    string Salt();

    string Hash(string sourceValue, string forSalt);
}