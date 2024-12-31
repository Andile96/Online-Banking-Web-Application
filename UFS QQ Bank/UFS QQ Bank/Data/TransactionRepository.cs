using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UFS_QQ_Bank.Data.DataAccess;
using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Data
{
    public class TransactionRepository:RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppEntityDbContext db) : base(db)
        {
        }

        public async Task AddTransaction(Transaction transaction)
        {
            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
        }
        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountNumber(string accountNumber)
        {
            return await _db.Transactions
                .Where(t => t.BankAccount.AccountNumber == accountNumber)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }


    }
}
