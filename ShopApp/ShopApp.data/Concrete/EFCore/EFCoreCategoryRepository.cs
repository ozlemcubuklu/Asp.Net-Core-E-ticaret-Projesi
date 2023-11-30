using Microsoft.EntityFrameworkCore;
using ShopApp.data.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.data.Concrete.EFCore
{
    public class EFCoreCategoryRepository : EfCoreGenericRepository<Category>, ICategoryRepository
    {
        public EFCoreCategoryRepository(ShopContext context) : base(context)
        {

        }
        private ShopContext ShopContext
        {
            get
            {
                return context as ShopContext;
            }
        }
        public void DeletefromCategory(int ProductId, int Id)
        {
           
                var cmd = "Delete from productcategory where ProductId=@p0 and CategoryId=@p1";
            ShopContext.Database.ExecuteSqlRaw(cmd,ProductId,Id);
            
        }

        public Category GetByIdWithProducts(int id)
        {
            
                return ShopContext.Categories.Where(i=>i.Id==id)
                    .Include(i=>i.ProductCategory)
                    .ThenInclude(i=>i.Product).FirstOrDefault();
            
        }

        public List<Category> GetPopularCategories()
        {
            throw new NotImplementedException();
        }
    }
}
