using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UFS_QQ_Bank.Data;
using UFS_QQ_Bank.Models;
using UFS_QQ_Bank.Models.ViewModels;

namespace UFS_QQ_Bank.Controllers
{
    [Authorize(Roles = "Consultant")]
    public class ConsultantController : Controller
    {

        private readonly IRepositoryWrapper _repository;
        public ConsultantController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }   

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SearchClient()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchClient(string accountNumber)
        {
            var account = await _repository.bankAccount.GetBankAccountByAccountNumber(accountNumber);

            if (account == null)
            {
                ViewBag.Message = "Client not found.";
                return View();
            }

            return RedirectToAction("ClientDetails", new { accountNumber = account.AccountNumber });
        }

        [HttpGet]
        public async Task<IActionResult> ClientDetails(string accountNumber)
        {
            
            var account = await _repository.bankAccount.GetBankAccountByAccountNumber(accountNumber);

            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> PerformTransaction(string accountNumber, decimal amount, string transactionType)
        {
            var account = await _repository.bankAccount.GetBankAccountByAccountNumber(accountNumber);

            if (account == null)
            {
                return NotFound();
            }

            var transaction = new Transaction
            {
                AccountID = account.AccountID,
                Amount = amount,
                TransactionType = transactionType,
                Reference = account.AccountHolder,
                BankAccount = account,
                TransactionDate = DateTime.Now
            };

            if (transactionType == "Withdraw")
            {
                if (account.AccountBalance < amount)
                {
                    ModelState.AddModelError("", "Insufficient balance.");
                    return View("ClientDetails", account);
                }

                account.AccountBalance -= amount;
                await _repository.bankAccount.UpdateAccount(account);
                await _repository.Transaction.AddTransaction(transaction);
                await _repository.Notification.AddNotification(account.AccountHolder, $"You have withdrawn {amount:C} from your account.");
               
                await _repository.ClientActivity.AddConsultantActivityAsync(new ClientActivity
                {
                    ConsultantId = User.Identity.Name, // Assuming you track consultantId
                    ClientName = account.AccountHolder,
                    ActivityType = "Withdrawal",
                    ActivityDate = DateTime.Now
                });
            }
            else if (transactionType == "Deposit")
            {
                account.AccountBalance += amount;
                await _repository.bankAccount.UpdateAccount(account);
                await _repository.Transaction.AddTransaction(transaction);
                await _repository.Notification.AddNotification(account.AccountHolder, $"You have deposited {amount:C} into your account.");
            }

            return RedirectToAction("ClientDetails", new { accountNumber = account.AccountNumber });
        }
        [HttpGet]
        public IActionResult GenerateReport()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> GenerateReport(ConsultantActivityReportViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                var activities = await _repository.ClientActivity.GetConsultantActivitiesAsync(model.ConsultantId, model.StartDate, model.EndDate);

                
                model.TotalClientsRegistered = activities.Count(a => a.ActivityType == "Client Registration");
                model.TotalClientInfoUpdated = activities.Count(a => a.ActivityType == "Update Client Info");
                model.TotalDepositsAssisted = activities.Count(a => a.ActivityType == "Deposit");
                model.TotalWithdrawalsAssisted = activities.Count(a => a.ActivityType == "Withdrawal");

               
                model.ClientActivities = activities.Select(a => new ClientActivityViewModel
                {
                    ClientName = a.ClientName,
                    ActivityType = a.ActivityType,
                    ActivityDate = a.ActivityDate
                }).ToList();

                
                await _repository.ClientActivity.SaveReportAsync(model);

             
                return View("ConsultantReportResult", model);
            }

            return View(model);
        }

        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewAllReports()
        {
          
            var reports = await _repository.ClientActivity.GetAllReportsAsync();
            return View("AdminViewReports", reports);
        }
    }
}
