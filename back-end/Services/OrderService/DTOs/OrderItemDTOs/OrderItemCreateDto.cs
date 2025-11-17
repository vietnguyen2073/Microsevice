namespace OrderService.DTOs.OrderItemDTOs
{
    public class OrderItemCreateDto
    {
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public string? ImageUrl { get; set; }
    }
}
