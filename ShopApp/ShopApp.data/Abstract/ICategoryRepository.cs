using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.data.Abstract
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        List<Category> GetPopularCategories();
        Category GetByIdWithProducts(int id);
        void DeletefromCategory(int ProductId, int Id);
    }
}
