using Application.Contract;
using Context;
using DTOs.Review;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ProductReviewRepository :Repository<ProductReview,int> , IProductReviewRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductReviewRepository(ApplicationDbContext context) : base(context)
        {

            this._context = context; 

        }

       
        public async Task<IQueryable<ProductReview>> GetallReviewByProductIdAsync(int proId)
        {
            return _context.ProductReviews.Where(x => x.productID == proId);
        }

        // get item by customer id and   item Id

        public async Task<ProductReview> GetReviewforUserAsync(string userID, int proId)
        {

            var rev = _context.ProductReviews.FirstOrDefault(i => i.CustomerID == userID && i.productID ==proId);

            if (rev == null)
            {
                return null;
            }

            return rev;
        }
    }
}
