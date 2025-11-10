using ReportService.Models;

namespace ReportService.Repository
{
    public interface IReportsRepository
    {
        Task<IEnumerable<Report>> GetAllAsync();
        Task<Report?> GetByIdAsync(Guid id);
        Task AddAsync(Report report);
        Task DeleteAsync(Report report);
        Task SaveChangesAsync();
    }
}
