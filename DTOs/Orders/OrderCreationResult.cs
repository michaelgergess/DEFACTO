using Model;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DTOs.Orders
{
    public class OrderCreationResult
    {
        public Order Order { get; init; }
        public int TotalQuantity { get; init; }
        public decimal TotalPrice { get; init; }
        public List<itemsInOrdercs> itemsInOrdercs { get; set; }
    }
}
