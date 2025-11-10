using OrderService.Models;

namespace OrderService.DTOs;

public class OrderDto
{
    public string CustomerName { get; set; } = null!;
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderUpdateDto
{
    public OrderStatus Status { get; set; }
}

public class OrderReadDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemReadDto> OrderItems { get; set; } = new();
}

public class OrderCreateDto
{
    public string CustomerName { get; set; } = null!;
    public List<OrderItemCreateDto> Items { get; set; } = new();
}
