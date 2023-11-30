using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using ShopApp.Business.Abstract;
using ShopApp.WebUi.Identity;
using ShopApp.WebUi.Models;
using Iyzipay;
using Iyzipay.Model.V2.Transaction;
using ShopApp.Entity;
using Newtonsoft.Json;

namespace ShopApp.WebUi.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartService _cartService;
        private UserManager<User> _userManager;
        private IOrderService _orderService;

        public CartController(ICartService cartService, UserManager<User> userManager, IOrderService orderService)
        {
            _cartService = cartService;
            _userManager = userManager;
            _orderService = orderService;

        }
        public IActionResult Index()
        {


            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            Console.WriteLine(cart);
            return View(new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            });

        }

        public IActionResult AddToCart(int productId, int quantity)
        {

            var userId = _userManager.GetUserId(User);

            _cartService.AddToCart(userId, productId, quantity);


            return RedirectToAction("Index");
        }

        [HttpPost]

        public IActionResult DeleteFromCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            _cartService.DeleteFromCart(userId, productId);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));
            var order = new OrderModel
            {
                cartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.Name,
                        Price = (double)i.Product.Price,
                        ImageUrl = i.Product.ImageUrl,
                        Quantity = i.Quantity
                    }).ToList()
                }
            };
            return View(order);
        }

        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {

            if (ModelState.IsValid)
            {

                var userId = _userManager.GetUserId(User);
                var cart = _cartService.GetCartByUserId(userId);
                model.cartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.Name,
                        Price = (double)i.Product.Price,
                        ImageUrl = i.Product.ImageUrl,
                        Quantity = i.Quantity


                    }).ToList()
                };

                var payment = PaymentProcess(model);
                if (payment.Status == "success")
                {
                    SaveOrder(model, payment, userId);
                    ClearCart(model.cartModel.CartId);
                    return View("Success");
                }
                else
                {
                    var msg = new AlertMessage()
                    {
                        Message = $"{payment.ErrorMessage}",
                        AlertType = "danger"
                    };
                    TempData["message"] = JsonConvert.SerializeObject(msg);
                }


            }


            return View(model);
        }

        private void ClearCart(int CartId)
        {
            _cartService.ClearCart(CartId);
        }

        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Order()
            {
                OrderNumber = new Random().Next(111111, 999999).ToString(),
                OrderState = EnumOrderState.completed,
                PaymentType = EnumPaymentType.CreditCard,
                PaymentId = payment.PaymentId,
                ConversationId = payment.ConversationId,
                OrderDate = new DateTime(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserId = userId,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email,
                City = model.City,
                Note = model.Note
            };
            order.OrderItems = new List<Entity.OrderItem>();
            foreach (var item in model.cartModel.CartItems)
            {
                var orderItem = new Entity.OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,

                };

                order.OrderItems.Add(orderItem);

            }
            _orderService.Create(order);
        }

        private Payment PaymentProcess(OrderModel model)
        {

            Options options = new Options();
            options.ApiKey = "sandbox-L9YeolXmsBDcg596FHS0uCN4gVW4taEd";
            options.SecretKey = "sandbox-yRv672LSBMl4OWpVk9BHK6RMZ0oigvhM";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.ConversationId = new Random().Next(111111111, 999999999).ToString();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "133456789";
            request.Price = model.cartModel.TotalPrice().ToString();
            request.PaidPrice = model.cartModel.TotalPrice().ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.CartName;
            paymentCard.CardNumber = model.CartNumber;
            paymentCard.ExpireMonth = model.ExpirationMonth;
            paymentCard.ExpireYear = model.ExpirationYear;
            paymentCard.Cvc = model.Cvc;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;


            // paymentCard.CardNumber = "5528790000000008";
            // paymentCard.ExpireMonth = "12";
            // paymentCard.ExpireYear = "2030";
            // paymentCard.Cvc = 123;
            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = model.FirstName;
            buyer.Surname = model.LastName;
            buyer.GsmNumber = model.Phone;
            buyer.Email = model.Email;
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = model.Address;
            buyer.Ip = "85.34.78.112";
            buyer.City = model.City;
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = model.FirstName;
            shippingAddress.City = model.City;
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = model.Address;
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = model.FirstName + " " + model.LastName;
            billingAddress.City = model.City;
            billingAddress.Country = "Turkey";
            billingAddress.Description = model.Address;
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();


            foreach (var item in model.cartModel.CartItems)
            {
                BasketItem firstBasketItem = new BasketItem();
                firstBasketItem.Id = item.ProductId.ToString();
                firstBasketItem.Name = item.Name;
                firstBasketItem.Category1 = "Telefon";

                firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                firstBasketItem.Price = (item.Price * item.Quantity).ToString();
                basketItems.Add(firstBasketItem);
            }

            request.BasketItems = basketItems;
            return Payment.Create(request, options);
        }


        public IActionResult GetOrders()
        {
            var userId = _userManager.GetUserId(User);

            var orders = _orderService.GetOrders(userId);

            var ordersList = new List<OrderListModel>();
            foreach (var order in orders)
            {
                var orderModel = new OrderListModel()
                {
                    OrderId=order.Id,
                    FirstName=order.FirstName,
                    OrderNumber=order.OrderNumber,
                    OrderDate=order.OrderDate,
                    Address=order.Address,
                    Phone=order.Phone,
                    LastName=order.LastName,
                    Email=order.Email,
                    City=order.City,
                    OrderState=order.OrderState,
                    OrderItems=order.OrderItems.Select(x => new OrderItemModel() { 
                    Name=x.Product.Name,
                    Price=x.Price,
                    Quantity=x.Quantity,
                    ImageUrl=x.Product.ImageUrl
                    }).ToList()



                };
                ordersList.Add(orderModel);


            };


            return View("Orders", ordersList);
        }


    }
}
