namespace UFS_QQ_Bank.Models.ViewModels
{
    public class ClientAccountDetailsViewModel
    {
        public BankAccount Account { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
