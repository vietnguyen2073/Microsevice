using PaymentService.Models;
using PaymentService.Data;
using Microsoft.EntityFrameworkCore;

namespace PaymentService.Repository
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentsRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<List<Payment>> GetAllAsync() => await _context.Payments.ToListAsync();
        public async Task<Payment?> GetByIdAsync(Guid id) => await _context.Payments.FindAsync(id);
        public async Task CreateAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Payment payment)
        {
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }
}
