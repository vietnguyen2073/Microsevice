namespace OrderService.DTOs
{
    public class OrderItemDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Amount {  get; set; }
        public string? ImageUrl { get; set; }

    }

    public class OrderItemReadDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class OrderItemCreateDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }   // số lượng
        public string? ImageUrl { get; set; }
    }
}
