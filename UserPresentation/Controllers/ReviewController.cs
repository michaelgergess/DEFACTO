using Application.Service.ItemReviewF;
using DTOs.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace UserPresentation.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IProductReviewService _productReviewService;
        private readonly UserManager<AppUser> _UserManager;
        public ReviewController(IProductReviewService productReviewService, UserManager<AppUser> userManager)
        {
            _productReviewService = productReviewService;
            _UserManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> rateItem(ProductReviewDTO rate)
        {

            if (User.Identity.IsAuthenticated && ModelState.IsValid)
            {
                var user = await _UserManager.GetUserAsync(User);
                rate.CustomerID = user?.Id;
                var result = await _productReviewService.rateItem(rate);
                if (result.IsSuccess)
                {
                    return Ok(result.Message);
                }
                return BadRequest(result.Message);
            }
            else
            {
                return BadRequest("Not Authorized");
            }


        }  
        [Authorize]
        public async Task<IActionResult> addReviewForproduct(ProductReviewDTO rate)
        {

            if (User.Identity.IsAuthenticated && ModelState.IsValid)
            {
                var user = await _UserManager.GetUserAsync(User);
                rate.CustomerID = user?.Id;
                var result = await _productReviewService.Create(rate);
                if (result.IsSuccess)
                {
                    TempData["ProductID"] = rate.productID;
                    return  RedirectToAction("Details", "product");
                }
                return BadRequest(result.Message);
            }
            else
            {
                return BadRequest("Not Authorized");
            }


        }
    }
}
