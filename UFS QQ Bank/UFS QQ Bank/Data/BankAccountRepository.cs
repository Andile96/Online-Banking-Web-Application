using Microsoft.EntityFrameworkCore;
using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Data
{
    public class BankAccountRepository : RepositoryBase<BankAccount>, IBankAccountRepository
    {

        
        public BankAccountRepository(AppEntityDbContext db) : base(db)
        {
           
        }

      
        public async Task AddBankAccount(BankAccount account)
        {
            await _db.BankAccounts.AddAsync(account);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<BankAccount>> GetAllBankAccount()
        {
            return await _db.BankAccounts.ToListAsync();
        }

        public async Task<BankAccount> GetBankAccountById(string username)
        {
            return await _db.BankAccounts.FirstOrDefaultAsync(a=>a.AccountHolder ==username);
        }
        public async Task<BankAccount> GetBankAccountInforId(string userId)
        {
            return await _db.BankAccounts
                .Include(b => b.Transactions) 
                .FirstOrDefaultAsync(a => a.AccountHolder == userId);
        }
        public async Task<bool> isAccountExist(string bankAccountNum)
        {
            return await _db.BankAccounts.AnyAsync(a => a.AccountNumber ==bankAccountNum);
        }
        public async Task<BankAccount> GetBankAccountByAccountNumber(string accountNumber)
        {
            return await _db.BankAccounts.FirstOrDefaultAsync(b => b.AccountNumber == accountNumber);
        }

        public async Task UpdateAccount(BankAccount account)
        {
            _db.BankAccounts.Update(account);
            await _db.SaveChangesAsync();
        }



    }
}
