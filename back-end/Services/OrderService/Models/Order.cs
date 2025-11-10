namespace OrderService.Models;

public enum OrderStatus
{
    Pending = 0,
    Paid = 1,
    Cancelled = 2
}


public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CustomerName { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<OrderItem> OrderItems = new();

    public void getTotalPrice()
    {
        TotalPrice = OrderItems.Sum(i => i.getTotalPrice());
    }
}