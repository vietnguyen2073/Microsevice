namespace OrderService.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; } 
        public string? ImageUrl { get; set; }

        //FK
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        public decimal getTotalPrice()
        {
            return Price * Amount;

        }
    }
}
