
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{ 
    // Appuser : baseentity :base : identity 

    public class AppUser : IdentityUser
    {

        //  acticve seller or not 
        public Gender? userGender { get; set; }
        public bool IsBlocked { get; set; }

        public string? ProfileImage { get; set; }
        [ForeignKey("address")]
        public int? addressID { get; set; }
        public Address address { get; set; }

        public ICollection<Order> orders { get; set; }
        public ICollection<ProductReview> productReviews { get; set; }
        public ICollection<Product> products { get; set; }

        public AppUser()
        {
            orders = new List<Order>();
            productReviews = new List<ProductReview>();
            products = new List<Product>();

        }


    }
}
