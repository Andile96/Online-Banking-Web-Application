using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UFS_QQ_Bank.Data;
using UFS_QQ_Bank.Models.ViewModels;
using UFS_QQ_Bank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace UFS_QQ_Bank.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientMenuController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserValidator<User> _userValidator;
        private readonly IRepositoryWrapper _wrapper;

        public ClientMenuController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserValidator<User> userValidator,
            IRepositoryWrapper wrapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userValidator = userValidator;
            _wrapper = wrapper;
        }
       
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

           
            var model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = user.Gender,
                EmployeeOrStudentNumber = user.EmployeeOrStudentNumber,
                Phone = user.PhoneNumber,
                UserType = user.UserType,
                IDOrPassportNumber = user.IDOrPassportNumber,
                DateOfBirth = user.DateOfBirth,
                Profile =user.Profile,
                ProfilePicture = null 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

     
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Gender = model.Gender;
            user.EmployeeOrStudentNumber = model.EmployeeOrStudentNumber;
            user.PhoneNumber = model.Phone;
            user.UserType = model.UserType;
            user.IDOrPassportNumber = model.IDOrPassportNumber;
            user.DateOfBirth = model.DateOfBirth;
            user.Profile = model.Profile;

           
            if (model.ProfilePicture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ProfilePicture.CopyToAsync(memoryStream);
                    user.Profile = memoryStream.ToArray(); 
                }
            }

    
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _wrapper.Notification.AddNotification(user.Id, "Your profile has been updated successfully.");
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                await _wrapper.Notification.AddNotification(user.Id, "Your password has been changed successfully.");
                ViewBag.Message = "Password has been changed successfully.LogIn again";
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AccountInfor()
        {
            // Get the current logged-in user's ID
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Fetch the user's bank account
            var account = await _wrapper.bankAccount.GetBankAccountById(user.UserName);
            if (account == null)
            {
                return NotFound();
            }

            // Fetch the transaction history for the account
            var transactions = await _wrapper.Transaction.GetTransactionsByAccountNumber(account.AccountNumber);

            // Pass the account and transaction data to the view
            var model = new ClientAccountDetailsViewModel
            {
                Account = account,
                Transactions = transactions
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Balance()
        {

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var account = await _wrapper.bankAccount.GetBankAccountById(user.UserName);

            if (account == null)
            {
                return NotFound("Bank account not found");
            }

            return View(account);
        }

        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userTask = _userManager.GetUserAsync(User);
                userTask.Wait();
                var user = userTask.Result;
                //var userId = await _userManager.FindByEmailAsync(User.Identity.Name);
                // var userId = User.fin(User.Identity.Name);
                var notifications = await _wrapper.Notification.GetNotifications(user.Id);

                ViewBag.Notifications = notifications.Take(2).ToList(); // Show the last 5 notifications
                
                ViewBag.UnreadNotificationsCount = notifications.Count(n => !n.IsRead);
              

                if (user != null)
                {
                    if (user.Profile != null && user.Profile.Length > 0)
                    {
                        ViewBag.ProfilePicture = $"data:image/jpeg;base64,{Convert.ToBase64String(user.Profile)}";
                    }
                    else
                    {
                        ViewBag.Initials = $"{user.FirstName?[0]}{user.LastName?[0]}".ToUpper();
                    }
                }
            }
        
            base.OnActionExecuting(context);
        }
        [HttpGet]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            await _wrapper.Notification.MarkAsRead(id);
            return View();
        }

        [HttpGet]
        public IActionResult TransferMoney()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> TransferMoney(string accountholder,string receiverAccountNumber, decimal amount, string reference)
        {
            
            var senderAccount = await _wrapper.bankAccount.GetBankAccountById(User.Identity.Name); 

            if (senderAccount == null)
            {
                ViewBag.Message = "Sender account not found.";
                return View();
            }

            var receiverAccount = await _wrapper.bankAccount.GetBankAccountByAccountNumber(receiverAccountNumber);

            if (receiverAccount == null)
            {
                ViewBag.Message = "Receiver account not found.";
                return View();
            }

            if (senderAccount.AccountBalance < amount)
            {
                ViewBag.Message = "Insufficient balance.";
                return View();
            }

            senderAccount.AccountBalance -= amount;
            await _wrapper.bankAccount.UpdateAccount(senderAccount);

            receiverAccount.AccountBalance += amount;
            await _wrapper.bankAccount.UpdateAccount(receiverAccount);

            var senderTransaction = new Transaction
            {

                AccountID = senderAccount.AccountID,
                TransactionType = "Payment",
                Amount = amount,
                Reference = reference,
                TransactionDate = DateTime.Now,
                BankAccount = senderAccount,
                description =senderAccount.AccountHolder
                
            };
            await _wrapper.Transaction.AddTransaction(senderTransaction);

            var receiverTransaction = new Transaction
            {
                AccountID = receiverAccount.AccountID,
                TransactionType = "Payment-Received",
                Amount = amount,
                Reference = reference,
                TransactionDate = DateTime.Now,
                BankAccount = receiverAccount
            };
            await _wrapper.Transaction.AddTransaction(receiverTransaction);

            await _wrapper.Notification.AddNotification(senderAccount.AccountHolder, $"You have sent {amount:C} to {receiverAccount.AccountNumber}.");
             await _wrapper.Notification.AddNotification(receiverAccount.AccountHolder, $"You have received {amount:C} from {senderAccount.AccountNumber}.");

            ViewBag.Message = "Transfer completed successfully.";
            return View();
        }
    }
}


