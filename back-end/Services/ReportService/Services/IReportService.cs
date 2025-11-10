using ReportService.DTOs;

namespace ReportService.Services
{
    public interface IReportService
    {
        Task<IEnumerable<ReportReadDto>> GetAllAsync();
        Task<ReportReadDto?> GetByIdAsync(Guid id);
        Task<ReportReadDto> CreateAsync(ReportCreateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
