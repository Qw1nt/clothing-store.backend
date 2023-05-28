using ClothingStore.Data.Context.Entities;

namespace ClothingStore.Data.Requests;

public class CheckoutRequest
{
    public string Name { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;
    
    public string? Comment { get; set; }

    public string Address { get; set; } = null!;

    public List<CartItem> CartItems { get; set; } = null!;
}