using ShopApp.Entity;
using System.Collections.Generic;

namespace ShopApp.WebUi.Models
{
    public class ProductDetailsModel
    {
        public Product Product { get; set; }
        public List<Category>  Categories { get; set; }
    }
}
