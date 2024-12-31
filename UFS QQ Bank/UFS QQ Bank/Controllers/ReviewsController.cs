using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UFS_QQ_Bank.Data;
using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly UserManager<User> _userManager;

        public ReviewsController(IRepositoryWrapper wrapper, UserManager<User> userManager)
        {
            _wrapper = wrapper;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult WriteReview()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WriteReview(Reviews model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            
            var review = new Reviews
            {
                UserId = user.Id,
                UserName = user.UserName,
                Rating = model.Rating,
                ReviewText = model.ReviewText,
            };

            await _wrapper.Reviews.AddReview(review);

            return RedirectToAction("ViewReviews");
        }

        [HttpGet]
        public async Task<IActionResult> ViewReviews()
        {
            var reviews = await _wrapper.Reviews.GetAllReviews();
            return View(reviews);
        }
    }
}

