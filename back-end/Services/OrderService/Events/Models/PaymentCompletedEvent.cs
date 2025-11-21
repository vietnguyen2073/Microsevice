using OrderService.Models;

namespace OrderService.Events.Models
{
    public class PaymentCompletedEvent
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
