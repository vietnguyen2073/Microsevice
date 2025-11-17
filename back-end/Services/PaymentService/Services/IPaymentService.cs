using PaymentService.DTOs;

namespace PaymentService.Services
{
    public interface IPaymentService
    {
        Task<List<PaymentDto>> GetAllAsync();
        Task<PaymentDto?> GetByIdAsync(Guid id);
        Task<PaymentDto> CreateAsync(PaymentCreateDto dto);
        Task<bool> UpdateAsync(Guid id, PaymentUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
