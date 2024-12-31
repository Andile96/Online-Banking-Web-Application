using System.ComponentModel.DataAnnotations;

namespace UFS_QQ_Bank.Models.ViewModels
{
    public class ConsultantActivityReportViewModel
    {
        public string ConsultantId { get; set; }
        public string ConsultantName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalClientsRegistered { get; set; }
        public int TotalClientInfoUpdated { get; set; }
        public int TotalDepositsAssisted { get; set; }
        public int TotalWithdrawalsAssisted { get; set; }
        public List<ClientActivityViewModel> ClientActivities { get; set; } = new List<ClientActivityViewModel>();
    }

    public class ClientActivityViewModel
    {
        public string ClientName { get; set; }
        public string ActivityType { get; set; }
        public DateTime ActivityDate { get; set; }
    }
}

