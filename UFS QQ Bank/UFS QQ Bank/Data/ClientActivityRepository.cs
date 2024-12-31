using Microsoft.EntityFrameworkCore;
using UFS_QQ_Bank.Models;
using UFS_QQ_Bank.Models.ViewModels;

namespace UFS_QQ_Bank.Data
{
    public class ClientActivityRepository : RepositoryBase<ClientActivity>, IClientActivityRepository
    {
        public ClientActivityRepository(AppEntityDbContext db) : base(db)
        {
        }
        public async Task AddConsultantActivityAsync(ClientActivity activity)
        {
            await _db.ClientActivities.AddAsync(activity);
            await _db.SaveChangesAsync();
        }
        public async Task<List<ClientActivity>> GetConsultantActivitiesAsync(string consultantId, DateTime startDate, DateTime endDate)
        {
            return await _db.ClientActivities
                .Where(a => a.ConsultantId == consultantId && a.ActivityDate >= startDate && a.ActivityDate <= endDate)
                .ToListAsync();
        }

        public async Task SaveReportAsync(ConsultantActivityReportViewModel report)
        {
            var reportEntity = new Reports
            {
                ConsultantId = report.ConsultantId,
                ConsultantName = report.ConsultantName,
                StartDate = report.StartDate,
                EndDate = report.EndDate,
                TotalClientsRegistered = report.TotalClientsRegistered,
                TotalClientInfoUpdated = report.TotalClientInfoUpdated,
                TotalDepositsAssisted = report.TotalDepositsAssisted,
                TotalWithdrawalsAssisted = report.TotalWithdrawalsAssisted
            };
            _db.Reports.Add(reportEntity);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ConsultantActivityReportViewModel>> GetAllReportsAsync()
        {
            return await _db.Reports
                .Select(r => new ConsultantActivityReportViewModel
                {
                    ConsultantId = r.ConsultantId,
                    ConsultantName = r.ConsultantName,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    TotalClientsRegistered = r.TotalClientsRegistered,
                    TotalClientInfoUpdated = r.TotalClientInfoUpdated,
                    TotalDepositsAssisted = r.TotalDepositsAssisted,
                    TotalWithdrawalsAssisted = r.TotalWithdrawalsAssisted
                })
                .ToListAsync();
        }
    }
}
