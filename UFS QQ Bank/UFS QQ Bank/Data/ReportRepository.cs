using Microsoft.EntityFrameworkCore;
using UFS_QQ_Bank.Models;
using UFS_QQ_Bank.Models.ViewModels;

namespace UFS_QQ_Bank.Data
{
    public class ReportRepository : RepositoryBase<Reports>, IReportRepository
    {
        public ReportRepository(AppEntityDbContext db) : base(db)
        {
        }
        public async Task AddConsultantReportAsync(Reports report)
        {
            await _db.Reports.AddAsync(report);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Reports>> GetAllConsultantReportsAsync()
        {
            return await _db.Reports.ToListAsync();
        }

        public async Task<Reports> GetConsultantReportByIdAsync(int reportId)
        {
            return await _db.Reports.FindAsync(reportId);
        }
    }
}
