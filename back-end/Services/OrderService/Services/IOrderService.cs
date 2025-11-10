using OrderService.DTOs;

namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderReadDto>> GetAllAsync();
        Task<OrderReadDto?> GetByIdAsync(Guid id);
        Task<OrderReadDto> CreateAsync(OrderCreateDto dto);
        Task<bool> UpdateAsync(Guid id, OrderUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
    