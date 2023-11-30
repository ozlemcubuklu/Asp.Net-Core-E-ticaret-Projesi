using ShopApp.Business.Abstract;
using ShopApp.data.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private  IOrderRepository _orderService;
        public OrderManager(IOrderRepository orderService)
        {
            _orderService = orderService;
        }
        public void Create(Order entity)
        {
            _orderService.Create(entity);
        }

        public Order GetById(int orderId)
        {
           return _orderService.GetById(orderId);
        }

        public List<Order> GetOrders(string userId)
        {
            return _orderService.GetOrders(userId);
        }
    }
}
