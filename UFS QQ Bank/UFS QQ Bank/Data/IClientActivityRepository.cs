using UFS_QQ_Bank.Models;
using UFS_QQ_Bank.Models.ViewModels;
using ClientActivity = UFS_QQ_Bank.Models.ClientActivity;

namespace UFS_QQ_Bank.Data
{
    public interface IClientActivityRepository: IRepositoryBase<ClientActivity>
    {
        Task<List<ClientActivity>> GetConsultantActivitiesAsync(string consultantId, DateTime startDate, DateTime endDate);
        Task SaveReportAsync(ConsultantActivityReportViewModel report);
        Task<List<ConsultantActivityReportViewModel>> GetAllReportsAsync();
        Task AddConsultantActivityAsync(ClientActivity activity);
    }
}
