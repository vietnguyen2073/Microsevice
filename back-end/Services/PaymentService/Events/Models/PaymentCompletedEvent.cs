using PaymentService.Models;

namespace PaymentService.Events.Models
{
    public class PaymentCompletedEvent
    {
        public Guid OrderId { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
