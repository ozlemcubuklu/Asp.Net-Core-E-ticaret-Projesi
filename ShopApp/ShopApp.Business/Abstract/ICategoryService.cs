using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Abstract
{
    public interface ICategoryService:IValidation<Category>
    {
        List<Category> GetAll();
        void Create(Category entity);
        void Update(Category entity);
        void Delete(Category entity);
        Category GetById(int id);
        Category GetByIdWithProducts(int id);
        void DeletefromCategory(int ProductId, int Id);
    }
}
