using Microsoft.EntityFrameworkCore;
using ShopApp.data.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;

namespace ShopApp.data.Concrete.EFCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product>, IProductRepository
    {
        public EfCoreProductRepository(ShopContext context) : base(context)
        {

        }
        private ShopContext ShopContext
        {
            get
            {
                return context as ShopContext;
            }
        }
        public Product GetByIdWithCategories(int id)
        {
           
                return ShopContext.Products.Where(i => i.ProductId == id)
                    .Include(i => i.ProductCategory)
                    .ThenInclude(i => i.Category)
                    .FirstOrDefault();
            
        }

        public int getCountByCategory(string category)
        {
            
                var product = ShopContext.Products.Where(i => i.IsApproved).AsQueryable();
                if (!string.IsNullOrEmpty(category))
                {
                    product = product.Include(i => i.ProductCategory).ThenInclude(i => i.Category)
                                            .Where(i => i.ProductCategory.Any(a => a.Category.Url == category));
                }
                return product.Count();
         }
        

        public List<Product> GethomePageProducts()
        {
           
                var product = ShopContext.Products.Where(i => i.IsApproved && i.IsHome).ToList();


                return product;
            
        }

        public List<Product> GetPopularProducts()
        {
            throw new NotImplementedException();
        }

        public Product GetProductDetails(string url)
        {
            
                return ShopContext.Products.Where(i=>i.Url==url)
                    .Include(i=>i.ProductCategory).ThenInclude(i=>i.Category)
                    .FirstOrDefault();  
            
        }

        public List<Product> GetProductsByCategory(string category,int page,int pagesize)
        {
            
                var product = ShopContext.Products.Where(i => i.IsApproved).AsQueryable();
                if (!string.IsNullOrEmpty(category)) {
                    product = product.Include(i => i.ProductCategory).ThenInclude(i => i.Category)
                                            .Where(i => i.ProductCategory.Any(a => a.Category.Url== category));
                }
                return product.Skip((page-1)*pagesize).Take(pagesize).ToList();
            
        }

        public List<Product> GetTop5Products()
        {
            throw new NotImplementedException();
        }

        public List<Product> SearchResult(string search)
        {
            
                var product = ShopContext.Products.Where(i => i.IsApproved && (i.Name.ToLower().Contains(search.ToLower()) || i.Description.ToLower().Contains(search.ToLower()))).AsQueryable();
                
                return product.ToList();
            
        }

        public void Update(Product entity, int[] categoryIds)
        {
            
                var product = ShopContext.Products.Include(i => i.ProductCategory).FirstOrDefault(i=>i.ProductId==entity.ProductId);
                if (product != null)
                {
                    product.Url = entity.Url;
                    product.Price=entity.Price;
                    product.Description=entity.Description;
                    product.Name= entity.Name;
                    product.ImageUrl= entity.ImageUrl;
                    product.IsApproved=entity.IsApproved;
                    product.IsHome=entity.IsHome;
                    product.ProductCategory = categoryIds
                        .Select(catId => new ProductCategory() { ProductId = entity.ProductId, CategoryId = catId }).ToList();

                }
                context.SaveChanges();
            
        }
    }
}
