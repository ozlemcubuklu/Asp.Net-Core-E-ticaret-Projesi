using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ShopApp.data.Abstract;

namespace shopapp.webui.ViewComponents
{
    public class CategoriesViewComponent:ViewComponent
    {
        private ICategoryRepository _categoryRepository;
        public CategoriesViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IViewComponentResult Invoke()
        {
            if (RouteData.Values["category"]!=null)
                ViewBag.SelectedCategory = RouteData?.Values["category"];
            //return View(CategoryRepository.Categories);
            return View(_categoryRepository.GetAll());
        }
    }
}