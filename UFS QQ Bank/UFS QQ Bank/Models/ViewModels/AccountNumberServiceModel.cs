using Microsoft.AspNetCore.Identity;
using UFS_QQ_Bank.Data;

namespace UFS_QQ_Bank.Models.ViewModels
{
    public class AccountNumberServiceModel
    {
      
        private readonly IRepositoryWrapper _wrapper;
        public AccountNumberServiceModel(IRepositoryWrapper wrapper)
        { 
            _wrapper = wrapper;
        }
        public async Task<string> AutoGenerateAccountNum()
        {
            Random random = new Random();
            string sAccountNum;
            bool isExists;

            do
            {
                sAccountNum = random.Next(1000000000, 1999999999).ToString();
                isExists = await _wrapper.bankAccount.isAccountExist(sAccountNum);
            } while (isExists);
           
            return sAccountNum;
        }
        public async Task CreateAccountNumUser(User user)
        {
            
            string sBankAccountNum = await AutoGenerateAccountNum();
            BankAccount bankAccount = new BankAccount
            {
                AccountNumber = sBankAccountNum,
                AccountHolder = user.UserName,
                AccountBalance = 0.0M,
                AccountOpenDate = DateTime.Now,
                AccountType = "Savings account"
            };
            await _wrapper.bankAccount.AddBankAccount(bankAccount);
            await _wrapper.SaveChanges();
            
            
        }
    }
}
