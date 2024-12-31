using System.ComponentModel.DataAnnotations;

namespace UFS_QQ_Bank.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }
        public string UserID { get; set; }
        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime Created_At { get; set; }

        [Required]
        public bool IsRead { get; set; }

        

    }
}
