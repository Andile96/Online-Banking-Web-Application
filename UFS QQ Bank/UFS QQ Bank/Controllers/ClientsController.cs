using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UFS_QQ_Bank.Data;
using UFS_QQ_Bank.Data.DataAccess;
using UFS_QQ_Bank.Models;
using UFS_QQ_Bank.Models.ViewModels;

namespace UFS_QQ_Bank.Controllers
{
    [Authorize(Roles = "Admin, Consultant")]
    public class ClientsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserValidator<User> _userValidator;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AccountNumberServiceModel _accountNumberServiceModel;
        private readonly IRepositoryWrapper _wrapper;
        

        private readonly string sRole = "Client";
        public ClientsController(UserManager<User> userManager,
            IUserValidator<User> userValidator,
            RoleManager<IdentityRole> roleManager,
            AccountNumberServiceModel accountNumberServiceModel,
            IRepositoryWrapper wrapper)
        {
            _userManager = userManager;
            _userValidator = userValidator;
            _roleManager = roleManager;
            _accountNumberServiceModel = accountNumberServiceModel;
            _wrapper = wrapper;
        }
        [TempData]
        public string sMessage { get; set; }
        public int iPageSize = 5;

        [HttpGet]
        public async Task<IActionResult> ClientUsers()
        {
            var client = await _wrapper.client.GetAllUsersDetails();
            return View(new AppViewModel
            {
                ClientUsers = client
            });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateClientUserModel registerModel)
        {

            string str = registerModel.FirstName.ToString().ToUpper();
            if (ModelState.IsValid)
            {

                if (await _roleManager.FindByNameAsync(sRole) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(sRole));
                }

                User user = new()
                {
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    Gender = registerModel.Gender,
                    IDOrPassportNumber = registerModel.IDPassportNumber,
                    DateOfBirth = registerModel.DateOfBirth,
                    Phone = registerModel.Phone,
                    EmployeeOrStudentNumber = registerModel.StaffStudentNumber,
                    UserName = registerModel.LastName + str.Substring(0, 1),
                    Email = registerModel.Email,
                    UserType = registerModel.UserType

                };

                IdentityResult result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    await _accountNumberServiceModel.CreateAccountNumUser(user);
                    await _userManager.AddToRoleAsync(user, sRole);
                    return RedirectToAction("ClientUsers", "Clients");
                }
                else
                {
                    foreach (var error in result.Errors.Select(x => x.Description))
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }

            return View(registerModel);
        }
        [HttpGet]
        public async Task<IActionResult> AccountInfo(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var bankAccount = await _wrapper.bankAccount.GetBankAccountById(user.UserName);
            if (bankAccount == null)
            {
                return NotFound();
            }

            var accountViewModel = new BankAccountViewModel
            {
                AccountNumber = bankAccount.AccountNumber,
                AccountType = bankAccount.AccountType,
                AccountBalance = bankAccount.AccountBalance,
                AccountHolder = bankAccount.AccountHolder,
                AccountOpenDate = bankAccount.AccountOpenDate
             
            };

            return View(accountViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var bankAccount = await _wrapper.bankAccount.GetBankAccountById(user.UserName);

            if (user == null || bankAccount == null)
            {
                return NotFound();
            }

            var deleteClientView = new DeleteClientViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                UserType = user.UserType,
                IDOrPassportNumber = user.IDOrPassportNumber,
                EmployeeOrStudentNumber = user.EmployeeOrStudentNumber,
                Phone = user.Phone,
                Email = user.Email,
                UserName = user.UserName
            };

           
            return View(deleteClientView);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var user = await _userManager.FindByEmailAsync(id);
            var bankAccount = await _wrapper.bankAccount.GetBankAccountById(user.UserName);

            if (user == null || bankAccount == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                await _wrapper.bankAccount.RemoveAsync(bankAccount.AccountID);
                await _userManager.RemoveFromRoleAsync(user, "Client");
                await _wrapper.SaveChanges();

                return RedirectToAction("ClientUsers", "Clients"); 
            }
            else
            {
                
                return View("Error"); 
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var model = new EditClientViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                UserType = user.UserType,
                IDPassportNumber = user.IDOrPassportNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Password = "" 
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit(string id, EditClientViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            // Update user properties
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Phone = model.Phone;
            user.UserType = model.UserType;
            user.IDOrPassportNumber = model.IDPassportNumber;
            user.DateOfBirth = model.DateOfBirth;
            user.Gender = model.Gender;
            user.UserName = model.LastName + model.FirstName.Substring(0, 1);

            IdentityResult validEmail = await _userValidator.ValidateAsync(_userManager, user);
            if (!validEmail.Succeeded)
            {
                AddErrorsFromResult(validEmail);
                return View(model);
            }

            IdentityResult validPass = null;
            if (!string.IsNullOrEmpty(model.Password))
            {
                if (await _userManager.HasPasswordAsync(user))
                {
                    await _userManager.RemovePasswordAsync(user);
                }

                validPass = await _userManager.AddPasswordAsync(user, model.Password);
                if (!validPass.Succeeded)
                {
                    AddErrorsFromResult(validPass);
                    return View(model);
                }
            }

            IdentityResult result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("ClientUsers", "Clients");
            }
            else
            {
                AddErrorsFromResult(result);
            }

            return View(model);
        }


        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
       
        //public IActionResult Index(string sortBy ="LastName",string searchString="", int page =1)  
        //{
        //    IEnumerable<User> client;
        //    Expression<Func<User, Object>> orderBy;
        //    string orderByDirection;
        //    int iTotalClient;

        //    ViewData["NameSort"] =  sortBy == "LastName" ? "LastName_desc" : "LastName";
        //    ViewData["DateSortParam"] = sortBy == "AccountOpenDate" ? "AccountOpenDate_desc" : "AccountOpenDate";
        //    ViewData["Message"] = searchString;

        //    if (string.IsNullOrEmpty(sortBy))
        //    {
        //        sortBy = "LastName";
        //    }

        //    if (sortBy.EndsWith("_desc"))
        //    {
        //        sortBy = sortBy.Substring(0, sortBy.Length - 5);
        //        orderByDirection = "desc";
        //    }
        //    else
        //    {
        //        orderByDirection = "asc";
        //    }
        //    orderBy = p => EF.Property<object>(p, sortBy);
        //    if (searchString == "")
        //    {
        //        iTotalClient =  _repository.client.GetAllAsync().;
        //        client = _repository.client.GetWithOptions(new QueryOptions<User>
        //        {
        //            OrderBy = orderBy,
        //            OrderByDirection = orderByDirection,
        //            Where = s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString),
        //            PageNumber = page,
        //            PageSize = iPageSize

        //        });
        //    }
        //    else
        //    {
        //        iTotalClient = _repository.client.FindByCondition(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString)).Count();
        //        client = _repository.client.GetWithOptions(new QueryOptions<User>
        //        {
        //            OrderBy = orderBy,
        //            OrderByDirection = orderByDirection,
        //            Where = s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString),
        //            PageNumber = page,
        //            PageSize = iPageSize
        //        });
        //    }

        //    return View(new ClientListViewModel
        //    {
        //        Users = client,
        //        PagingInfo = new PagingInfoViewModel
        //        {
        //            CurrentPage = page,
        //            ItemsPerPage = iPageSize,
        //            TotalItems = iTotalClient
        //        }
        //    });
        //}
    }
}
