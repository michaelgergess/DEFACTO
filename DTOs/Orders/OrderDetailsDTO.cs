using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Orders
{
    public class OrderDetailsDTO
    {
        public int Id { get; init; }
        public OrderStatus Status { get; init; }
        public decimal TotalAmount { get; init; }
        public int TotalQuantity { get; init; }
        public DateTime CreatedDate { get; init; }
    }
}
