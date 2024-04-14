using Application.Contract;
using Application.Contracts;
using AutoMapper;
using DTO_s.ViewResult;
using DTOs.Review;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.ItemReviewF
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _ProductReviewRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _UserManager;
        private readonly IMapper _mapper;

        public ProductReviewService(IProductReviewRepository ProductReviewRepository
            , IProductRepository productRepository, IUserRepository userRepository
            , IMapper mapper, UserManager<AppUser> userManager)
        {
            _ProductReviewRepository = ProductReviewRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _UserManager = userManager;
            _mapper = mapper;

        }
        public async Task<ResultView<ProductReviewDTO>> Create(ProductReviewDTO review)
        {
            var product = await _productRepository.GetByIdAsync(review.productID);
            var user = await _UserManager.FindByIdAsync(review.CustomerID);
            if (user == null || product == null)
            {
                return new ResultView<ProductReviewDTO>
                {
                    IsSuccess = false,
                    Entity = null,
                    Message = "not valid item or user "
                };
            }
            // if found the review update 
            var foundRev = await _ProductReviewRepository.GetReviewforUserAsync(review.CustomerID,review.productID);
            if (foundRev != null)
            {
                foundRev.ReviewMessage = review.ReviewMessage;

                await _ProductReviewRepository.SaveChangesAsync();

            }

            var productReview = _mapper.Map<ProductReview>(review);
            await _ProductReviewRepository.CreateAsync(productReview);
            await _ProductReviewRepository.SaveChangesAsync();

            return new ResultView<ProductReviewDTO>
            {
                Entity = review,
                IsSuccess = true,
                Message = "Add review  Successfully"
            };

        }
       
        public async Task<ResultDataList<ProductReviewListDTO>> ProductReviews(int proID)
        {
            var itemlist = await _ProductReviewRepository.GetallReviewByProductIdAsync(proID);

            var list = _mapper.Map<List<ProductReviewListDTO>>(itemlist);

            return new ResultDataList<ProductReviewListDTO>
            {

                Entities = list,
                Count = list.Count()
            };
        }

        public async Task<ResultView<ProductReviewDTO>> rateItem(ProductReviewDTO review)
        {

            var item = await _productRepository.GetByIdAsync(review.productID);
            var user = await _UserManager.FindByIdAsync(review.CustomerID);
            if (user == null || item == null)
            {
                return new ResultView<ProductReviewDTO>
                {
                    IsSuccess = false,
                    Entity = null,
                    Message = "not valid item or user "
                };
            }
            // get item by customer id and   item Id
            var foundRev = await _ProductReviewRepository.GetReviewforUserAsync(review.CustomerID, review.productID);
            if (foundRev != null)
            {
                foundRev.ReviewRate = review.ReviewRate;
                await _ProductReviewRepository.SaveChangesAsync();

            }
            var product = _mapper.Map<ProductReview>(review);
            await _ProductReviewRepository.CreateAsync(product);
            await _ProductReviewRepository.SaveChangesAsync();

            return new ResultView<ProductReviewDTO>
            {
                Entity = review,
                IsSuccess = true,
                Message = "Rate  Successfully"
            };



        }

       
    }
}
