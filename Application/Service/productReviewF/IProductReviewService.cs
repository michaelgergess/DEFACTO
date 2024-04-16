using Context.Migrations;
using DTO_s.ViewResult;
using DTOs.Item;
using DTOs.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ItemReviewF
{
    public interface IProductReviewService
    {
        // GET REWVIEW ber product by proid 
        Task<ResultDataList<ProductReviewListDTO>> ProductReviews(int proID);
        // add review for item  
        Task<ResultView<ProductReviewDTO>> Create(ProductReviewDTO Item);

        // rate item 1-5 
        Task<ResultView<ProductReviewDTO>> rateItem(ProductReviewDTO Item);


    }
}
