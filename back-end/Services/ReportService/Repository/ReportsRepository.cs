using Microsoft.EntityFrameworkCore;
using ReportService.Data;
using ReportService.Models;

namespace ReportService.Repository;

public class ReportsRepository : IReportsRepository
{
    private readonly ReportDbContext _context;

    public ReportsRepository(ReportDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Report>> GetAllAsync() =>
        await _context.Reports
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

    public async Task<Report?> GetByIdAsync(Guid id) =>
        await _context.Reports.FindAsync(id);

    public async Task AddAsync(Report report) =>
        await _context.Reports.AddAsync(report);

    public async Task DeleteAsync(Report report)
    {
        _context.Reports.Remove(report);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
