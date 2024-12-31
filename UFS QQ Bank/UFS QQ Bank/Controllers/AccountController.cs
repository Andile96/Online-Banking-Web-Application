using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using UFS_QQ_Bank.Models;
using UFS_QQ_Bank.Models.ViewModels;

namespace UFS_QQ_Bank.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AccountNumberServiceModel _accountNumberServiceModel;

        private readonly string sRole = "Client";
        public AccountController(UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager,
         AccountNumberServiceModel accountNumberServiceModel)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _accountNumberServiceModel = accountNumberServiceModel;
          
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
      public async Task<IActionResult> Login(LoginModel loginModel)
{
    if (ModelState.IsValid)
    {
    
        User user = await _userManager.FindByEmailAsync(loginModel.Email);

        if (user != null)
        {
           
            var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, false);

            
            if (result.Succeeded)
            {
                return Redirect(loginModel?.ReturnUrl ?? "/Home/Index");
            }

           
            if (!result.Succeeded && user.MobilePassword != null)
            {
                
                var hashedMobilePassword = _userManager.PasswordHasher.HashPassword(user, loginModel.Password);

                
                if (hashedMobilePassword == user.MobilePassword)
                {
                    
                    return Redirect(loginModel?.ReturnUrl ?? "/Home/Index");
                }
            }
        }
    }

    ModelState.AddModelError("", "Invalid email or password");
    return View(loginModel);
}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
           
            string str = registerModel.FirstName.ToString().ToUpper();
            if (ModelState.IsValid)
            {

                if(await _roleManager.FindByNameAsync(sRole)==null)
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
                user.MobilePassword = registerModel.Password;
            
                IdentityResult result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    await _accountNumberServiceModel.CreateAccountNumUser(user);
                    await _userManager.AddToRoleAsync(user,sRole);
                    return RedirectToAction("Login");
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
