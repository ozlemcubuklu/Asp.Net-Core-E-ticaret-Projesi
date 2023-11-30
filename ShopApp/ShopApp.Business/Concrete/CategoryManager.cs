using ShopApp.Business.Abstract;
using ShopApp.data.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        
        public void Create(Category entity)
        {
           _categoryRepository.Create(entity);
        }

        

        public void Delete(Category entity)
        {
           _categoryRepository.Delete(entity);
        }

        public void DeletefromCategory(int ProductId, int Id)
        {
            _categoryRepository.DeletefromCategory(ProductId,Id);
        }

        public List<Category> GetAll()
        {
         return _categoryRepository.GetAll();
        }

        public Category GetById(int id)
        {
          return _categoryRepository.GetById(id);
        }

        public Category GetByIdWithProducts(int id)
        {
           return _categoryRepository.GetByIdWithProducts(id);
        }

        public void Update(Category entity)
        {
            _categoryRepository.Update(entity);
        }
        public string ErrorMessage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Validation(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
