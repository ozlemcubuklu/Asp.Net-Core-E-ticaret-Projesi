using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.data.Abstract
{
    public interface ICartRepository:IGenericRepository<Cart>
    {
        void ClearCart(int cartId);
        void DeleteFromCart(int CartId, int productId);
        Cart GetCartByUserId(string userId);
    }
}
