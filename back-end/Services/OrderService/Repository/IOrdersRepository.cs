using OrderService.Models;

namespace OrderService.Repository
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task CreateAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
        Task<bool> SaveChangesAsync();
    }
}
