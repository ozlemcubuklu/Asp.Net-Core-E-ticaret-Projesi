using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopApp.Business.Abstract;
using ShopApp.Entity;
using ShopApp.WebUi.Models;
using System.Linq;

namespace ShopApp.WebUi.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;
        public ShopController(IProductService productservice)
        {
            this._productService = productservice;
        }
        public IActionResult List(string category,int page=1)
        {
            const int pagesize =2;
            var productViewModel = new ProductListViewModel()
            {
                PageInfo=new PageInfo() { ItemsPerPage=pagesize,
                                          TotalItems=_productService.getCountByCategory(category),
                                          CurrentPage=page,
                                          CurrentCategory=category},
                Products = _productService.GetProductsByCategory(category,page,pagesize)
            };

            return View(productViewModel);
        }
        public IActionResult Details(string url) {
            if (url==null)
            {
                return NotFound();
            }
            Product p = _productService.GetProductDetails(url);
            if (p==null)
            {
                return NotFound();
            }

            return View(new ProductDetailsModel { Product=p,Categories=p.ProductCategory.Select(i=>i.Category).ToList()});
        }
        public IActionResult Search(string q)
        {
            var productViewModel = new ProductListViewModel()
            {

                Products = _productService.SearchResult(q)
            };

            return View(productViewModel);
        }
    }
}
