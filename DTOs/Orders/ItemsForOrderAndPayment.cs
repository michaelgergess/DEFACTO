using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Orders
{
    public class ItemsForOrderAndPayment
    {
        public List<itemsInOrdercs> itemsInOrdercs { get; set; }
        public int totalQuantity { get; set; }
        public decimal totalPrice { get; set; }
    }
}
