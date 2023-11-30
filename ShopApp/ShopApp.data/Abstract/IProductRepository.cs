using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.data.Abstract
{
  
        public interface IProductRepository : IGenericRepository<Product>
        {
        List<Product> GetProductsByCategory(string category,int page,int pagesize);
            Product GetProductDetails(string url);
            List<Product> GetPopularProducts();
            List<Product> GetTop5Products();
            int getCountByCategory(string category);
        List<Product> GethomePageProducts();
        List<Product> SearchResult(string search);
        Product GetByIdWithCategories(int id);
        void Update(Product entity, int[] categoryIds);
    }
    
}
