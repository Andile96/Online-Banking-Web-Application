using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Data
{
    public interface IBankAccountRepository :IRepositoryBase<BankAccount>
    {
        Task<BankAccount> GetBankAccountById(string username);
        Task<BankAccount> GetBankAccountInforId(string userId);
        Task<IEnumerable<BankAccount>> GetAllBankAccount();
        Task AddBankAccount(BankAccount account);
        Task<bool> isAccountExist(string bankAccountNum);
        Task<BankAccount> GetBankAccountByAccountNumber(string accountNumber);
        Task UpdateAccount(BankAccount account);



    }
}
