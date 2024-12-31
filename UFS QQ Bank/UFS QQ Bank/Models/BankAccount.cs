using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UFS_QQ_Bank.Models
{
    public class BankAccount
    {
        [Key]
        public int AccountID { get; set; }

        [Required]
        public string AccountHolder { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string AccountType { get; set; } //debit or credit
        [Required]
        public decimal AccountBalance { get; set; }
        [Required]
        public DateTime AccountOpenDate { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
