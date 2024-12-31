using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using UFS_QQ_Bank.Data;
using UFS_QQ_Bank.Models;
using UFS_QQ_Bank.Models.ViewModels;
using UFS_QQ_Bank.Infrastructure;

namespace UFS_QQ_Bank.Controllers
{

    public class NotificationsController : Controller
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly UserManager<User> _userManager;

        public NotificationsController(IRepositoryWrapper wrapper, UserManager<User> userManager)
        {
            _wrapper = wrapper;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AllNotifications()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    
                    var notifications = await _wrapper.Notification.GetNotifications(user.Id);

                    var viewModel = new NotificationViewModel
                    {
                        Notifications = notifications 
                    };

                    return View(viewModel); 
                }
                await _wrapper.Notification.MarkAsRead(user.Id);
            }

            return RedirectToAction("Login", "Account");
        }


    }
}
