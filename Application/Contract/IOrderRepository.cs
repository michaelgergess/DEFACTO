using Application.Contracts;
using DTOs.Orders;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contract
{
    public interface IOrderRepository : IRepository<Order, int>
    {
        IQueryable<OrderDetailsDTO> GetOrderDetailsBy(string CustomerId);
        Task<decimal> GetTotalPriceBY(OrderItemDTO orderItemDTO);
            }
}
