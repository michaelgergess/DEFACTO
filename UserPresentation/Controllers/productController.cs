﻿using Application.Services;
using DTO_s.Product;
using DTO_s.ViewResult;
using DTOs.Product;
using DTOs.ProductFilter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text;

namespace UserPresentation.Controllers
{
    public class productController : Controller
    {
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;
        public productController(IProductService productService, IConfiguration configuration)
        {
            _productService = productService;
            _configuration = configuration;

        }


        /// <summary>
        /// / don't forget th change subcatgory
        /// </summary>
        /// <param name="subCategory"></param>
        /// <param name="GetAllProductBySubcategory"></param>
        /// <returns></returns>
        public async Task<IActionResult> ProductsFilter(SubCategory subCategory, string searchTxt, string catName, int PageNo = 1)
        {
            ResultDataList<ProductForFitlter> list = await _productService.GetAllProductBySubcategory(subCategory, searchTxt, catName, PageNo);
            var model = new productsFilterWithPaging
            {
                resultDataList = list,
                searchTxt = searchTxt,
                subCategory = subCategory,
                CurrentPage = PageNo,
                itemsForPage = 12
            };
            return View(model);
        }
        // make model to take any filter - 
        [HttpPost]
        public async Task<IActionResult> ProductsFilterByAJax(AjaxFilterDTO FilterModel, string searchTxt, int PageNo = 1)
        {                                                                // filter with model 
            ResultDataList<ProductForFitlter> list = await _productService.GetAllProductByFilterModel(FilterModel, searchTxt, PageNo);
            var model = new productsFilterWithPaging
            {
                resultDataList = list,
                searchTxt = searchTxt,
                subCategory = FilterModel.subCategory,
                CurrentPage = PageNo,
                itemsForPage = 12
            };

            return PartialView("_itemFilterPartialView", model);
        }

  

        [HttpGet]

        public async Task<IActionResult> Details(int ProID)
        {
            ResultView<ProductDetailsDTO> productDetails ;
            if (ProID <= 0)
            {
                var IdFromLocal = TempData["ProductID"];
                if(IdFromLocal != null && (int)IdFromLocal > 0)
                {
                     productDetails = await _productService.GetProductDetails((int)IdFromLocal);
                    if (productDetails.Entity == null)
                    {
                        return RedirectToAction("ProductsFilter");
                    }
                    return View(productDetails.Entity);
                }
                
                return RedirectToAction("ProductsFilter");
            }
             productDetails = await _productService.GetProductDetails(ProID);
            if(productDetails.Entity == null)
            {
                return RedirectToAction("ProductsFilter");
            }
            return View(productDetails.Entity);
        }
        [HttpPost]
        public async Task<IActionResult> Favorite(string[] favorites)
        {
            List<ProductFavoriteDTO> model;
            if (favorites.IsNullOrEmpty() || favorites.FirstOrDefault() == "null" )
            {
                model = new List<ProductFavoriteDTO>()
                {
                   
                };
                return View(model);
            }
            var favorite = await _productService.ProductsInFavorite(favorites.ToList());
            var favoritesJson = JsonSerializer.Serialize(favorite.Entities);

            // Store favorites in session instead of TempData
            HttpContext.Session.SetString("Favorites", favoritesJson);
             model = favorite.Entities;
            return View("Favorite", model);
        }
        public async Task<IActionResult> GetFavorites(string[] favorites)
        {
            var favorite = await _productService.ProductsInFavorite(favorites.ToList());
            var favoritesJson = JsonSerializer.Serialize(favorite.Entities);

            // Store favorites in session instead of TempData
            HttpContext.Session.SetString("Favorites", favoritesJson);

            return PartialView("_Favorits", favorite.Entities);
        }
        public async Task<IActionResult> FilterFavorites(string query)
        {
           var favoritesJson = HttpContext.Session.GetString("Favorites"); // Retrieve favorites from session

            if (favoritesJson != null)
            {
                var listOfProduct = JsonSerializer.Deserialize<List<ProductFavoriteDTO>>(favoritesJson);

                if (!string.IsNullOrEmpty(query))
                {
                    // Filter products based on the query asynchronously
                    var filteredProducts = await _productService.GetFilteredProducts(query, listOfProduct);
                    return PartialView("_Favorits", filteredProducts);
                }

                // Return all favorites if query is empty
                return PartialView("_Favorits", listOfProduct);
            }

            return NotFound(); 

        }

        public ActionResult Cart()
        {
            return View();
        }
        public async Task<IActionResult> GetCartItems(string[] carts)
        {
           
            var Cart = await _productService.ProductsInCart(carts.ToList());
            ViewBag.CartEntities = Cart.Entities;
            
            return PartialView ("_CartPage"); 
        }

        public async Task<JsonResult> GetSizes(int ProductId, string ColorName)
        {
            var sizes = await _productService.GetProductsSizesBy(ProductId, ColorName);
            return Json(sizes);
        }
        public async Task<JsonResult> GetColors(int ProductId, string SizeName)
        {
            var colors = await _productService.GetProductsSizesBy(ProductId, SizeName);
            return Json(colors);
        }
        public async Task<JsonResult> GetQuantityAndPrice(int ProductId, string ColorName,string? SizeName)
        {
            var QuantityAndPrice = await _productService.GetProductsQuantityBy(ProductId, ColorName, SizeName);

            return Json(QuantityAndPrice);
        } 
           
    }
}   
