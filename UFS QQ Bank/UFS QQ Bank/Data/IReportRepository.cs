using UFS_QQ_Bank.Models;
using UFS_QQ_Bank.Models.ViewModels;

namespace UFS_QQ_Bank.Data
{
    public interface IReportRepository: IRepositoryBase<Reports>
    {
        Task AddConsultantReportAsync(Reports report);
        Task<IEnumerable<Reports>> GetAllConsultantReportsAsync();
        Task<Reports> GetConsultantReportByIdAsync(int reportId);
        
    }
}
