using PaymentService.Models;

namespace PaymentService.Repository
{
    public interface IPaymentsRepository
    {
        Task<List<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(Guid id);
        Task CreateAsync(Payment payment);
        Task DeleteAsync(Payment payment);
    }
}
