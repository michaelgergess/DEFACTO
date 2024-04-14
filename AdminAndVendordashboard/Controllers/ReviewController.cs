
using Application.Service.ItemReviewF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminAndVendordashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ReviewController : ControllerBase
    {
        private readonly IProductReviewService productReviewService;

        public ReviewController(IProductReviewService productReviewService)
        {
            this.productReviewService = productReviewService;
        }
        [HttpGet]

        public async Task<IActionResult> getreviewBerpeoduct(int pro_id)
        {
            var model = await productReviewService.ProductReviews(pro_id);
            if (model.Entities == null || model.Count == 0)
            {
                return BadRequest(ModelState);
            }
            return Ok(model);

        }
    }
}
