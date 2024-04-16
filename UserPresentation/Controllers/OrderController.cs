using Application.Service.Order;
using Application.Services;
using DTOs.Orders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model;
using Stripe.Checkout;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Text;
using NuGet.Protocol.Core.Types;

namespace UserPresentation.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<AppUser> _UserManager;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, UserManager<AppUser> userManager, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _UserManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(string paymentmethod, Session session)
        {
            if (!string.IsNullOrEmpty(paymentmethod))
            {
                string Cash = "cash";
                string Credit_Card = "credit-card";
                if (HttpContext.Session.TryGetValue("SelectedItems", out byte[] selectedItemsBytes))
                {
                    var selectedItems = JsonSerializer.Deserialize<List<OrderItemDTO>>(selectedItemsBytes);


                    var ItemsSelected = await _orderService.FilterItemBySelected(selectedItems);
                    var user = await _UserManager.FindByNameAsync(User.Identity.Name);

                    if (ItemsSelected.Count > 0 && user is not null)
                    {
                        if (paymentmethod == Credit_Card)
                        {

                            var result = await _orderService.CreateItems(ItemsSelected);
                            var res = await _orderService.Payment(result.itemsInOrdercs);
                            var checkOnPaidId = res.Id.ToString();
                            HttpContext.Session.SetString("CheckOnPaid", checkOnPaidId);

                            Response.Headers.Add("Location", res.Url);
                            return new StatusCodeResult(303);
                        }
                        else if (paymentmethod == Cash)
                        {
                            var OrderProduct = await _orderService.GetProductIdsBy(ItemsSelected);
                            var cartsJson = JsonSerializer.Serialize(OrderProduct);

                            var cartsBytes = Encoding.UTF8.GetBytes(cartsJson);

                            HttpContext.Session.Set("Carts", cartsBytes);
                            var result = await _orderService.CreateOrder(ItemsSelected, user.Id);
                        return    RedirectToAction("OrderDetails");

                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                   
                    {
                        return RedirectToAction("Cart", "Product");
                    }
                   
                }
                else
                {
                    return NotFound();
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> OrderInfo(List<OrderItemDTO> selectedItems)
        {
            if (!selectedItems.Any())
            {
                byte[] selectedItemsBytes = HttpContext.Session.Get("SelectedItems");

                if (selectedItemsBytes != null)
                {
                    selectedItems = JsonSerializer.Deserialize<List<OrderItemDTO>>(selectedItemsBytes);
                }
                else
                {
                    selectedItems = new List<OrderItemDTO>();
                }
            }
            else
            {
                // If selectedItems is not null, serialize and store it in the session
                byte[] selectedItemsBytes = JsonSerializer.SerializeToUtf8Bytes(selectedItems);
                HttpContext.Session.Set("SelectedItems", selectedItemsBytes);
            }

            // Continue with the rest of your logic
            var ItemsSelected = await _orderService.FilterItemBySelected(selectedItems);
            var TotalPrice = await _orderService.GetTotalPrice(ItemsSelected);
            ViewBag.TotalPrice = TotalPrice;
            return View();
        }

        public async Task<IActionResult> OrderDetails(int PageNumber)
        {
            var user = await _UserManager.FindByNameAsync(User.Identity.Name);
            if (user is not null)
            {
                var cartsBytes = HttpContext.Session.Get("Carts");
                if (cartsBytes != null)
                {
                    string[] productIds = Encoding.UTF8.GetString(cartsBytes).Split(',');
                    ViewBag.ProductIds = productIds;
                }


                var Orders = await _orderService.GetOrderDetailsBy(user.Id, PageNumber, 5);
                return View(Orders);
            }

            return NotFound();
        }

        public async Task<IActionResult> OrderConfirmation()
        {
            var checkOnPaidId = HttpContext.Session.GetString("CheckOnPaid");
            var user = await _UserManager.FindByNameAsync(User.Identity.Name);
            if (!string.IsNullOrEmpty(checkOnPaidId)&& user is not null)
            {
                var service = new SessionService();
                Session session = service.Get(checkOnPaidId);

                if (session.PaymentStatus == "paid")
                {
                    if (HttpContext.Session.TryGetValue("SelectedItems", out byte[] selectedItemsBytes))
                    {
                        var selectedItems = JsonSerializer.Deserialize<List<OrderItemDTO>>(selectedItemsBytes);
                        var ItemsSelected = await _orderService.FilterItemBySelected(selectedItems);
                        var OrderProduct = await _orderService.GetProductIdsBy(ItemsSelected);
                        var cartsJson = JsonSerializer.Serialize(OrderProduct);

                        var cartsBytes = Encoding.UTF8.GetBytes(cartsJson);

                        HttpContext.Session.Set("Carts", cartsBytes);
                        var OrdersCreated = await _orderService.CreateOrder(ItemsSelected, user.Id);
                   return RedirectToAction("OrderDetails");
                    } }
            }

            return NoContent();
        }



    }
}
