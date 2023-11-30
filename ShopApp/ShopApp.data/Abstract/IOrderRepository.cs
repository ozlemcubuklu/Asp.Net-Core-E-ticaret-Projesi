using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.data.Abstract
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        List<Order> GetOrders(string userId);
    }
}
