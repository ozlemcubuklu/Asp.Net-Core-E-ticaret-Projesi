using Microsoft.EntityFrameworkCore;
using ShopApp.data.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.data.Concrete.EFCore
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order>, IOrderRepository
    {
        public EfCoreOrderRepository(ShopContext context) : base(context)
        {

        }
        private ShopContext ShopContext
        {
            get
            {
                return context as ShopContext;
            }
        }
        public List<Order> GetOrders(string userId)
        {
           
                var orders = ShopContext.Orders.Include(i => i.OrderItems).ThenInclude(i => i.Product).AsQueryable();

                if (!string.IsNullOrEmpty(userId)) {
                
                    orders=orders.Where(i => i.UserId == userId);
                }
                return orders.ToList();

            
        }

        public override Order GetById(int orderId)
        {
           
                var order = ShopContext.Orders.Include(i => i.OrderItems).ThenInclude(i => i.Product).AsQueryable().Where(i => i.Id == orderId).FirstOrDefault();


                return order;
            
        }
       
    }
}
