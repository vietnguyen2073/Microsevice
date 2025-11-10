using ReportService.DTOs;
using ReportService.Models;
using ReportService.Repository;

namespace ReportService.Services;

public class ReportService : IReportService
{
    private readonly IReportsRepository _repo;

    public ReportService(IReportsRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<ReportReadDto>> GetAllAsync()
    {
        var reports = await _repo.GetAllAsync();
        return reports.Select(r => new ReportReadDto
        {
            Id = r.Id,
            Title = r.Title,
            Content = r.Content,
            CreatedAt = r.CreatedAt
        });
    }

    public async Task<ReportReadDto?> GetByIdAsync(Guid id)
    {
        var report = await _repo.GetByIdAsync(id);
        if (report == null) return null;

        return new ReportReadDto
        {
            Id = report.Id,
            Title = report.Title,
            Content = report.Content,
            CreatedAt = report.CreatedAt
        };
    }

    public async Task<ReportReadDto> CreateAsync(ReportCreateDto dto)
    {
        var report = new Report
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(report);
        await _repo.SaveChangesAsync();

        return new ReportReadDto
        {
            Id = report.Id,
            Title = report.Title,
            Content = report.Content,
            CreatedAt = report.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var report = await _repo.GetByIdAsync(id);
        if (report == null) return false;

        await _repo.DeleteAsync(report);
        return true;
    }
}
