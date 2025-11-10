namespace PaymentService.Models
{

    public enum PaymentStatus
    {
        Pending,
        Success,
        Failed
    }

    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
