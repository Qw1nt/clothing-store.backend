namespace Domain.Common;

public class OrderInHistory
{
    public List<CartItem> Items { get; set; } = new();

    public DateTime Date { get; set; }
}