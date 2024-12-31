using System.ComponentModel.DataAnnotations;

namespace UFS_QQ_Bank.Models
{
    public class Reports
    {
        [Key]
        public int ReportId { get; set; }
        public string ConsultantId { get; set; } 
        public string ConsultantName { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalClientsRegistered { get; set; }
        public int TotalClientInfoUpdated { get; set; }
        public int TotalDepositsAssisted { get; set; }
        public int TotalWithdrawalsAssisted { get; set; }
    }
}
