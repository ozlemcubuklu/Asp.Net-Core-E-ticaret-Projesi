using Microsoft.EntityFrameworkCore;
using ShopApp.data.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.data.Concrete.EFCore
{
    public class EfCoreCartRepository : EfCoreGenericRepository<Cart>, ICartRepository
    {
        public EfCoreCartRepository(ShopContext context):base(context) 
        {
            
        }
        private ShopContext ShopContext {
            get {
                return context as ShopContext;} 
        }
        public void ClearCart(int cartId)
        {
          
                var cmd = "delete from CartItems where CartId=@p0";
            ShopContext.Database.ExecuteSqlRaw(cmd, cartId);
            
        }

        public void DeleteFromCart(int CartId, int productId)
        {
            var cmd = "delete from CartItems where CartId=@p0 and ProductId=@p1";
            ShopContext.Database.ExecuteSqlRaw(cmd,CartId,productId);
            
        }

        public Cart GetCartByUserId(string userId)
        {
           
                return ShopContext.Carts.Include(i => i.CartItems).ThenInclude(i => i.Product).FirstOrDefault(i=>i.UserId==userId);
            
        }

        public override void Update(Cart t)
        {

            ShopContext.Carts.Update(t);
            ShopContext.SaveChanges();
            
        }
    }
}
