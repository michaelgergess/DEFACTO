using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Review
{
    public class ProductReviewListDTO
    {
        public string CustomerName{ get; set; }
        public string ProductTitle{ get; set; }
        [Required]
        public string ReviewMessage { get; set; }
        [Range(1, 5)]
        public int? ReviewRate { get; set; }
        [Required]
        public int productID { get; set; }
    }
}
