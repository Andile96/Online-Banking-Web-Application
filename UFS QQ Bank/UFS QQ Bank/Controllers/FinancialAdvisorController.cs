using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UFS_QQ_Bank.Data;
using UFS_QQ_Bank.Models.ViewModels;
using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Controllers
{
    [Authorize(Roles = "Financial advisor,Admin")]
    public class FinancialAdvisorController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserValidator<User> _userValidator;
        private readonly IRepositoryWrapper _wrapper;



        public FinancialAdvisorController(UserManager<User> userManager,
            IUserValidator<User> userValidator,
            RoleManager<IdentityRole> roleManager,
            AccountNumberServiceModel accountNumberServiceModel,
            IRepositoryWrapper wrapper)
        {
            _userManager = userManager;
            _userValidator = userValidator;
            _wrapper = wrapper;
        }
        public  ActionResult Index()
        {


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> BankAccounts()
        {

            var bankAccounts = await _wrapper.bankAccount.GetAllBankAccount();


            return View(bankAccounts);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {

            var bankAccount = await _wrapper.bankAccount.GetBankAccountById(id);

            if (bankAccount == null)
            {
                return NotFound();
            }


            return View(bankAccount);
        }
    
        [HttpGet("/FinancialAdvisor/AddAdvice/{accountNumber}")]
        
        public async Task<IActionResult> AddAdvice(string accountNumber)
        {
            var account = await _wrapper.bankAccount.GetBankAccountByAccountNumber(accountNumber);

            if (account == null)
            {
                return NotFound();
            }

            var model = new AdviceViewModel
            {
                AccountNumber = account.AccountNumber,
                userID = account.AccountHolder 
            };

            return View(model);
        }

       
        [HttpPost]
        public async Task<IActionResult> AddAdvice(AdviceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var advice = new Advice
                {
                    AdvisorId = User.Identity.Name, 
                    AccountHolderId = model.userID,
                    AccountNumber = model.AccountNumber,
                    Comment = model.Comment,
                    DateGiven = DateTime.Now
                };

                await _wrapper.Advices.AddAdviceAsync(advice);
                return RedirectToAction("BankAccounts");
            }

            return View(model);
        }

      
        [HttpGet]
        public async Task<IActionResult> ViewAllAdvice()
        {
            var advices = await _wrapper.Advices.GetAllAdvicesAsync();
            return View(advices);
        }
    }
}
