namespace UFS_QQ_Bank.Models
{
    public class Advice
    {
        public int AdviceId { get; set; }
        public string AdvisorId { get; set; } 
        public string AccountHolderId { get; set; } 
        public string AccountNumber { get; set; } 
        public string Comment { get; set; } 
        public DateTime DateGiven { get; set; } = DateTime.Now;
    }
}
