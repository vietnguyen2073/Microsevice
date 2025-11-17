namespace PaymentService.DTOs
{
    public class PaymentCreateDto
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}
