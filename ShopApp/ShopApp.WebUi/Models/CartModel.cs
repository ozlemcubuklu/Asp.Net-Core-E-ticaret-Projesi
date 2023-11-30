using System.Collections.Generic;
using System.Linq;

namespace ShopApp.WebUi.Models
{
    public class CartModel
    {

        public int CartId { get; set; }
        public List<CartItemModel> CartItems { get; set; }
        public double TotalPrice()
        {
            return CartItems.Sum(i => i.Quantity * i.Price);
        }
    }

    public class CartItemModel
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}
