namespace OrderService.DTOs.OrderItemDTOs
{
    public class OrderItemReadDto
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
