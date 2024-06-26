﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Review
{
    public class ProductReviewDTO
    {
        [AllowNull]
        public string? CustomerID { get; set; }
       
        public string? ReviewMessage { get; set; }
        [Range(1, 5)]
        public int? ReviewRate { get; set; }
        [Required]
        public int productID { get; set; }
    }
}
