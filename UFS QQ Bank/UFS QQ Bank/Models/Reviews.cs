namespace UFS_QQ_Bank.Models
{
    public class Reviews
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public string UserName { get; set; } 
        public int Rating { get; set; } 
        public string ReviewText { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
