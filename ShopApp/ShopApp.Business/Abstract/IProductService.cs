using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Abstract
{
    public interface IProductService : IValidation<Product>
    {
        List<Product> GetProductsByCategory(string category, int page, int pagesize);
        List<Product> GetAll();
        bool Create(Product entity);
        void Update(Product entity);
        void Delete(Product entity);
        Product GetById(int id);
        Product GetProductDetails(string url);
        int getCountByCategory(string category);
        List<Product> GethomePageProducts();
        List<Product> SearchResult(string q);
        Product GetByIdWithCategories(int id);
        bool Update(Product entity, int[] categoryIds);
    }
}
