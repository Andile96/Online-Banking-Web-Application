using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Data
{
    public interface ITransactionRepository:IRepositoryBase<Transaction>
    {
        Task AddTransaction(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransactionsByAccountNumber(string accountNumber);
    }
}
