using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ClothingStore.Configurations;

public class AuthenticationConfiguration 
{
    public const string SectionKey = "AuthenticationSection";

    private SymmetricSecurityKey? _symmetricSecurityKey;
    
    public string Issuer { get; set; }  = string.Empty;
    
    public string Audience { get; set; } = string.Empty;
    
    public string BearerKey { get; set; } = string.Empty;
    
    public int LifeTime { get; set; }

    public SymmetricSecurityKey SymmetricSecurityKey()
    {
        return _symmetricSecurityKey ??= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(BearerKey));
    }
}