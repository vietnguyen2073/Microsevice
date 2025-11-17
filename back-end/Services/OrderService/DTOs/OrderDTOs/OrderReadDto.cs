using OrderService.DTOs.OrderItemDTOs;
using OrderService.Models;

namespace OrderService.DTOs.OrderDTOs
{
    public class OrderReadDto
    {
        public Guid Id { get; set; }
        public string? CustomerName { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemReadDto> OrderItems { get; set; } = new();
    }
}
