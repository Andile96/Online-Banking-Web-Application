using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UFS_QQ_Bank.Data;
using UFS_QQ_Bank.Models;
using UFS_QQ_Bank.Models.ViewModels;

namespace UFS_QQ_Bank.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
       
        private readonly UserManager<User> _userManager;
        private readonly IUserValidator<User> _userValidator;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AccountNumberServiceModel _accountNumberServiceModel;
        private readonly IRepositoryWrapper _wrapper;


        public AdminController(UserManager<User> userManager, 
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
        public async Task<IActionResult> Index()
        {

            var users = _userManager.Users.ToList();
            var filteredUsers = new List<User>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin") ||
                    await _userManager.IsInRoleAsync(user, "Consultant") ||
                    await _userManager.IsInRoleAsync(user, "Financial advisor"))
                {
                    filteredUsers.Add(user);
                }
            }

            return View(filteredUsers);
        }
       
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeUserViewModel model)
        {
            string str = model.FirstName.ToString();
            if (ModelState.IsValid)
            {
               
                User user = new()
                {
                    UserName = model.LastName.ToString() + str.Substring(0, 1),
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserType = model.Role,
                    EmployeeOrStudentNumber = model.StaffStudentNumber
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                user.MobilePassword = model.Password.ToString();

                if (result.Succeeded)
                {
                    
                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        var roleResult = await _roleManager.CreateAsync(new IdentityRole(model.Role));
                        if (!roleResult.Succeeded)
                        {
                            ModelState.AddModelError("", "Failed to create role");
                            return View(model);
                        }
                    }
                   

                    if (result.Succeeded)
                    {
                        
                        if (!string.IsNullOrEmpty(model.Role))
                        {
                            await _userManager.AddToRoleAsync(user, model.Role);
                            
                           
                        }
                        

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index");
        }
        public async Task<IActionResult> Edit(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail
                    = await _userValidator.ValidateAsync(_userManager, user);

                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }

                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    if (await _userManager.HasPasswordAsync(user))
                    {
                        await _userManager.RemovePasswordAsync(user);
                    }

                    validPass = await _userManager.AddPasswordAsync(user, password);

                    if (!validPass.Succeeded)
                    {
                        AddErrorsFromResult(validPass);
                    }
                }

                if ((validEmail.Succeeded && validPass == null)
                    || (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }

        
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
