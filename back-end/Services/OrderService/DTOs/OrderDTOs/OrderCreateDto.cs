using OrderService.DTOs.OrderItemDTOs;

namespace OrderService.DTOs.OrderDTOs
{
    public class OrderCreateDto
    {
        public string CustomerName { get; set; } = null!;
        public List<OrderItemCreateDto> Items { get; set; } = new();
    }
}
