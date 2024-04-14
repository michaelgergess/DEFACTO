using Application.Contract;
using AutoMapper;
using DTO_s.ViewResult;
using DTOs.Orders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model;
using DTOs.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe.BillingPortal;
using Stripe.Checkout;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;
using Azure;
using Application.Contracts;
using Stripe.Climate;
using DTOs.Paginated;

namespace Application.Service.Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IItemRepository itemRepository, IColorRepository colorRepository, ISizeRepository sizeRepository, IProductRepository productRepository, IImageRepository imageRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
            _productRepository = productRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<ItemsForOrderAndPayment> CreateItems(List<OrderItemDTO> items)
        {
            var itemsInOrder = new List<itemsInOrdercs>();
            var totalQuantity = 0;
            decimal totalPrice = 0;
            if (items.Any())
            {
                foreach (var selectedItem in items)
                {
                    var item = await _itemRepository.GetByIdAsync(selectedItem.itemID);
                    var itemTotalPrice = selectedItem.QuantityOfItem * item.Price;

                    var itemInOrder = new itemsInOrdercs
                    {
                        itemID = item.Id,
                        QuantityOfItem = selectedItem.QuantityOfItem,
                        TotalpriceForItem = itemTotalPrice,
                        item = item
                    };

                    var edititem = await _itemRepository.GetByIdAsync(itemInOrder.itemID);
                    edititem.Quantity -= itemInOrder.QuantityOfItem;
                    totalQuantity += selectedItem.QuantityOfItem;
                    totalPrice += itemTotalPrice;

                    itemsInOrder.Add(itemInOrder);
                }
            }
            return new ItemsForOrderAndPayment { 
                totalPrice = totalPrice ,
                itemsInOrdercs = itemsInOrder,
                totalQuantity = totalQuantity
            };
        }
        public async Task<OrderCreationResult> CreateOrder(List<OrderItemDTO> items, string customerId)

        {

            var itemsInOrder = await CreateItems(items);
            await _itemRepository.SaveChangesAsync();

            var order = new Model.Order()
            {
                CustomerID = customerId,
                dateTime = DateTime.Now,
                itemsInOrdercs = itemsInOrder.itemsInOrdercs,
                Quantity = itemsInOrder.totalQuantity,
                Totalprice = itemsInOrder.totalPrice
            };

            await _orderRepository.CreateAsync(order);
            await _orderRepository.SaveChangesAsync();


            var res = itemsInOrder;
            return new OrderCreationResult
            {
                itemsInOrdercs = itemsInOrder.itemsInOrdercs,
                Order = order,
                TotalQuantity = itemsInOrder.totalQuantity,
                TotalPrice = itemsInOrder.totalPrice
            };
             
           
           
        }

        public async Task<List<OrderItemDTO>> FilterItemBySelected(List<OrderItemDTO> items)
        {
            var selectedItems = await _itemRepository.FilterBySelected(items);

            return selectedItems;

        }
        public async Task<Stripe.Checkout.Session> Payment(List<itemsInOrdercs> items)
        {
            var domain = "http://localhost:5239/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + "Order/OrderConfirmation",
                CancelUrl = domain + "",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in items)
            {
                var getItem = await _itemRepository.GetByIdAsync(item.itemID);
                var product = await _productRepository.GetByIdAsync(getItem.productID);
               
                var itemName = $"{product.Title} ";

                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(getItem.Price * 100),
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = itemName,
                        }
                    },
                    Quantity = item.QuantityOfItem
                };

                options.LineItems.Add(sessionListItem);
            }

            var service = new SessionService();
            return await service.CreateAsync(options);
        }
        public async Task<List<OrderDetailsDTO>> GetOrderDetailsBy(string CustomerId,int PageNumber,int PageSize)
        {
            var res =   _orderRepository.GetOrderDetailsBy(CustomerId);
            var respage = await PaginatedList<OrderDetailsDTO>.CreateAsync(res, PageNumber, PageSize);

            return respage;

        }
        public async Task<List<int>> GetProductIdsBy(List<OrderItemDTO> orderItemDTO)
        {
            List<int> productsIds = new List<int>();
            foreach (var item in orderItemDTO)
            {
              productsIds.Add( await _productRepository.GetProductIdsBY(item));
            }
            return productsIds;
        }



        public async Task<ResultDataList<getallOrdersDTO>> getallDefactoOrdersAsync(int items =20, int pagenumber = 1)
        {
             var allOrders =   (await _orderRepository.GetAllAsync()).Include(o => o.User).Include(o => o.itemsInOrdercs).ThenInclude(i=>i.item);
            var orders = allOrders.Where(o=>o.IsDeleted==false|| o.IsDeleted==null)
                .Skip(items * (pagenumber - 1)).Take(items).ToList();



            if (!allOrders.Any())
            {
                return new ResultDataList<getallOrdersDTO> { Count = 0, Entities = null };
            }
             var ordersDTO =_mapper.Map<List<getallOrdersDTO>>(allOrders);
            return new ResultDataList<getallOrdersDTO>
            {
                Count = orders.Count(),
                Entities = ordersDTO
            };
        }

        public async Task<(bool, string)> changeOrderState(int OrderID, OrderStatus Status)
        {
            var order = await _orderRepository.GetByIdAsync(OrderID);
            if (order == null)
            {
                return (false ,"Not correct order number");
            }
            order.State = Status;
            await _orderRepository.SaveChangesAsync();
            return (true, "order Status changed ");
        }
        public async Task<decimal> GetTotalPrice(List<OrderItemDTO> orderItemDTO)
        {
            decimal TotalPrice = 0 ;
            foreach (var item in orderItemDTO)
            {
               TotalPrice += await _orderRepository.GetTotalPriceBY(item);
            }
            return  TotalPrice;
        }

    }

}

    

