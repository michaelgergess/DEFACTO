using Application.Contract;
using Context;
using DTOs.Orders;
using DTOs.Paginated;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class OrderRepository:Repository<Order, int>, IOrderRepository
    {
   
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext Context) : base(Context)
        {
            _context = Context;
        }
        public IQueryable<OrderDetailsDTO> GetOrderDetailsBy(string CustomerId)
        {
            return _context.orders
                .Where(o => o.CustomerID == CustomerId)
                .Select(o => new OrderDetailsDTO
                {
                    CreatedDate = o.dateTime,
                    Id = o.Id,
                    Status = o.State,
                    TotalAmount = o.Totalprice,
                    TotalQuantity = o.Quantity
                });
        }
        public async Task<decimal> GetTotalPriceBY(OrderItemDTO orderItemDTO)
        {
            var res = _context.items.
                Where(i => i.Id == orderItemDTO.itemID).
                Select(i => i.Price*orderItemDTO.QuantityOfItem).
                FirstOrDefault();
            return res;
        }
    }
}
