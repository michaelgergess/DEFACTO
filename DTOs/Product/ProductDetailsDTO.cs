using DTOs.Item;
using DTOs.Review;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Product
{
    public class ProductDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Gender? productGender { get; set; }
        public string? Code { get; set; }
        public decimal Price { get; set; }
        public string[] ImagesArr { get; set; } = new string[4];
        public string? CategoryName { get; set; }
        public string Description { get; set; }
        public List<ItemDto> items { get; set; }
        public List<Color>? Colors { get; set; }
        public List<Size>? Sizes { get; set; }
        public List<ProductReviewListDTO>  productReviewLists { get; set; }
        public string? ar_Title { get; set; }
        public string? ar_Description { get; set; }
        public string? Category_ar_Name { get; set; }
    }
}
