using PaymentService.DTOs;
using PaymentService.Events.Interface;
using PaymentService.Events.Models;
using PaymentService.Models;
using PaymentService.Repository;

namespace PaymentService.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentsRepository _repo;
        private readonly IRabbitMQPublisher _rabbitMQPublisher;

        public PaymentService(IPaymentsRepository repo, IRabbitMQPublisher rabbitMQPublisher)
        {
            _repo = repo;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        public async Task<List<PaymentDto>> GetAllAsync()
        {
            var payments = await _repo.GetAllAsync();
            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                OrderId = p.OrderId,
                Amount = p.Amount,
                Status = p.Status,
                CreatedAt = p.CreatedAt
            }).ToList();
        }

        public async Task<PaymentDto?> GetByIdAsync(Guid id)
        {
            var payment = await _repo.GetByIdAsync(id);
            if (payment == null) return null;
            return new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Status = payment.Status,
                CreatedAt = payment.CreatedAt
            };
        }

        public async Task<PaymentDto> CreateAsync(PaymentCreateDto dto)
        {
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = dto.OrderId,
                Amount = dto.Amount,
                Status = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.CreateAsync(payment);

            var result = new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Status = payment.Status,
                CreatedAt = payment.CreatedAt
            };

            //RabbitMQ
            var paymentEvent = new PaymentCompletedEvent
            {
                OrderId = result.Id,
                Status = PaymentStatus.Success
            };

            _rabbitMQPublisher.Publish(paymentEvent, exchangeName: "payment_exchange", routingKey: "payment.update");

            return result;
        }

        public async Task<bool> UpdateAsync(Guid id, PaymentUpdateDto dto)
        {
            var payment = await _repo.GetByIdAsync(id);
            if (payment == null) return false;

            payment.Status = dto.Status;

            await _repo.UpdateAsync(payment);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var payment = await _repo.GetByIdAsync(id);
            if (payment == null) return false;
            await _repo.DeleteAsync(payment);
            return true;
        }
    }
}
