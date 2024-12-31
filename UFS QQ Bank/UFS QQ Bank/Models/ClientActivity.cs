namespace UFS_QQ_Bank.Models
{
    public class ClientActivity
    {
        public int Id { get; set; }
        public string ConsultantId { get; set; }
        public string ClientName { get; set; }
        public string ActivityType { get; set; } // "Client Registration", "Deposit", "Withdrawal", etc.
        public DateTime ActivityDate { get; set; }
    }
}
