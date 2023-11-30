using ShopApp.Business.Abstract;
using ShopApp.data.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository _productRepository;
        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }



        public void Delete(Product entity)
        {
            _productRepository.Delete(entity);
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productRepository.GetByIdWithCategories(id);
        }

        public int getCountByCategory(string category)
        {
            return _productRepository.getCountByCategory(category);
        }

        public List<Product> GethomePageProducts()
        {
            return (_productRepository.GethomePageProducts());
        }

        public Product GetProductDetails(string url)
        {
            return _productRepository.GetProductDetails(url);
        }

        public List<Product> GetProductsByCategory(string category, int page, int pagesize)
        {
            return _productRepository.GetProductsByCategory(category, page, pagesize);
        }

        public List<Product> SearchResult(string q)
        {
            return _productRepository.SearchResult(q);
        }

        public void Update(Product entity)
        {
            _productRepository.Update(entity);
        }

        public bool Update(Product entity, int[] categoryIds)
        {
            if (Validation(entity))
            {
                if (categoryIds.Length==0)
                {
                    ErrorMessage += "Lütfen bir kategori seçiniz.";
                    return false;

                }
                
                _productRepository.Update(entity, categoryIds);
                return true; 
            }
           
            return false;
        }

        public string ErrorMessage { get ; set; }

        public bool Validation(Product entity)
        {
            var isValid = true;
            if (string.IsNullOrEmpty(entity.Name))
            {

                isValid = false;
                ErrorMessage += "ürün ismi girmelisiniz.\n";
            }

            if (entity.Price < 0)
            {
                isValid = false;
                ErrorMessage += "ürün fiyatı sıfırdan küçük olamaz.\n";
            }
            return isValid;
        }

        public bool Create(Product entity)
        {
            if (Validation(entity))
            {
                _productRepository.Create(entity);
                return true;
            }
            return false;

        }
    }
}
