using DTOs.Item;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Product
{
    public class ProductFavoriteDTO
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Image { get; init; }
        public decimal price { get; init; }

        public List<ItemDto>? items { get; init; }

        public string? ar_Title { get; init; }
        public string? ar_Description { get; init; }



    }
}
