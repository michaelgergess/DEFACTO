using Application.Contracts;
using DTOs.Review;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contract
{
    public interface IProductReviewRepository : IRepository<ProductReview, int>
    {
        Task<IQueryable<ProductReview>> GetallReviewByProductIdAsync(int proId);
        Task<ProductReview> GetReviewforUserAsync(string userID , int proId );
    }
}
