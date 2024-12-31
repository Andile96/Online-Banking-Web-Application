using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace UFS_QQ_Bank.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        public int AccountID { get; set; }
        public string TransactionType { get; set; }//Deposit, Withdrawals, payment
        public decimal Amount { get; set; }
        public string description { get; set; }///sender information
        public string Reference { get; set; }
        public DateTime TransactionDate { get; set; }

        public BankAccount BankAccount { get; set; }
    }
}

