using DTO_s.ViewResult;
using DTOs.Orders;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Order
{
    public interface IOrderService
    {
        // get all order for defacto
        Task<ResultDataList<getallOrdersDTO>> getallDefactoOrdersAsync(int items = 20, int pagenumber = 1);
        // get items for order 

        // change order stat 
        Task<(bool, string)> changeOrderState(int OrderID, OrderStatus Status);
        Task<List<OrderDetailsDTO>> GetOrderDetailsBy(string CustomerId, int PageNumber, int PageSize);
        Task<OrderCreationResult> CreateOrder(List<OrderItemDTO> items, string customerId);
        Task<List<OrderItemDTO>> FilterItemBySelected(List<OrderItemDTO> items);
        Task<Stripe.Checkout.Session> Payment(List<itemsInOrdercs> items);
        Task<ItemsForOrderAndPayment> CreateItems(List<OrderItemDTO> items);
        Task<List<int>> GetProductIdsBy(List<OrderItemDTO> orderItemDTO);
        Task<decimal> GetTotalPrice(List<OrderItemDTO> orderItemDTO);
    }
}
