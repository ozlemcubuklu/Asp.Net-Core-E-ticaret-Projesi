using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.Entity;
using ShopApp.WebUi.Extensions;
using ShopApp.WebUi.Identity;
using ShopApp.WebUi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ShopApp.WebUi.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private Business.Abstract.IOrderService _orderService;
        private IProductService _productService;
        private ICategoryService _categoryService;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        public AdminController(IProductService productService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, Business.Abstract.IOrderService orderService)
        {

            _roleManager = roleManager;
            _userManager = userManager;
            _productService = productService;
            _categoryService = categoryService;
            _orderService = orderService;
        }

        public IActionResult ProductList()
        {

            return View(new ProductListViewModel()
            {
                Products = _productService.GetAll()
            });
        }
        public IActionResult OrderEdit(int id)
        {
            var order=_orderService.GetById((int)id);
            var ordermodel=new OrderListModel() { 
                Address = order.Address,
                FirstName= order.FirstName,
                LastName= order.LastName,
                Phone= order.Phone,
                Email= order.Email,
                PaymentType= order.PaymentType,
                OrderDate= order.OrderDate,
                OrderNumber= order.OrderNumber,
                OrderState= order.OrderState,
                City= order.City,
                UserId= order.UserId,
                OrderId=order.Id,
                Note=order.Note,

                OrderItems=order.OrderItems.Select(i=>new OrderItemModel() {
                    Price=i.Price,
                    ImageUrl=i.Product.ImageUrl,
                    Quantity=i.Quantity,
                    Name=i.Product.Name
                }).ToList()
            
            
            };
            return View(ordermodel);
        }
        public IActionResult OrderList()
        {
            var orderList = _orderService.GetOrders(null);

            var orderlistmodel = new List<OrderListModel>();
            foreach (var order in orderList)
            {
                var ordermodel = new OrderListModel()
                {
                    OrderId = order.Id,
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,
                    Address = order.Address,
                    Email = order.Email,
                    City = order.City,
                    Phone = order.Phone,
                    PaymentType = order.PaymentType,
                    FirstName = order.FirstName,
                    LastName = order.LastName,
                    OrderState = order.OrderState,


                    OrderItems = order.OrderItems.Select(x => new OrderItemModel()
                    {
                        OrderItemId = x.Id,
                        Name = x.Product.Name,
                        Price = x.Price,
                        Quantity = x.Quantity,
                        ImageUrl = x.Product.ImageUrl
                    }).ToList()
                };
                orderlistmodel.Add(ordermodel);
            }


            return View(orderlistmodel);
        }

        public IActionResult CategoryList()
        {

            return View(new CategoryListViewModel()
            {
                Categories = _categoryService.GetAll()
            });
        }





        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {

            if (ModelState.IsValid)
            {
                Product p = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Url = model.Url,
                    ImageUrl = model.ImageUrl,
                    Price = model.Price
                };
                if (_productService.Create(p))
                {
                    TempData.Put("message", new AlertMessage() { AlertType = "success", Title = "Hesap Onayı", Message = "Hesabınız onaylanmıştır." });

                    return RedirectToAction("ProductList");
                }
                TempData.Put("message", new AlertMessage() { AlertType = "danger", Title = "Hesap Onayı", Message = _productService.ErrorMessage });

                return View(model);


            }
            return View(model);

        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                Category c = new Category()
                {
                    Name = model.Name,

                    Url = model.Url,

                };
                _categoryService.Create(c);

                TempData.Put("message", new AlertMessage() { AlertType = "success", Title = "Kategori ekleme", Message = $"{c.Name} adlı category eklendi" });


                return RedirectToAction("CategoryList");
            }
            return View(model);

        }



        [HttpGet]
        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _productService.GetByIdWithCategories((int)id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Description = entity.Description,
                Url = entity.Url,
                ImageUrl = entity.ImageUrl,
                Price = entity.Price,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity.ProductCategory.Select(i => i.Category).ToList()

            };
            ViewBag.categories = _categoryService.GetAll();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model, int[] categoryIds, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    return NotFound(ModelState);

                }
                var entity = _productService.GetById(model.ProductId);

                if (entity == null)
                {
                    return BadRequest(ModelState);
                }

                entity.ProductId = model.ProductId;
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Url = model.Url;

                entity.Price = model.Price;
                entity.IsApproved = model.IsApproved;
                entity.IsHome = model.IsHome;

                if (file != null)
                {
                    var extention = Path.GetExtension(file.FileName);
                    var randomname = string.Format($"{Guid.NewGuid()}{extention}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomname);
                    entity.ImageUrl = randomname;

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                if (_productService.Update(entity, categoryIds))
                {
                    TempData.Put("message", new AlertMessage() { AlertType = "success", Title = "Ürün güncellemesi", Message = $"{entity.Name} adlı ürün güncellendi" });


                    return RedirectToAction("ProductList");
                }
                TempData.Put("message", new AlertMessage() { AlertType = "danger", Title = "Hata", Message = _productService.ErrorMessage });



            }
            ViewBag.categories = _categoryService.GetAll();
            return View(model);

        }





        [HttpGet]
        public IActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _categoryService.GetByIdWithProducts((int)id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {

                Name = entity.Name,
                Id = entity.Id,
                Url = entity.Url,
                Products = entity.ProductCategory.Select(i => i.Product).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    return NotFound(ModelState);

                }
                var entity = _categoryService.GetById(model.Id);

                if (entity == null)
                {
                    return BadRequest(ModelState);
                }

                entity.Name = model.Name;
                entity.Url = model.Url;
                entity.Id = model.Id;
                _categoryService.Update(entity);

                TempData.Put("message", new AlertMessage() { AlertType = "success", Title = "Kategori güncellemesi", Message = $"{entity.Name} adlı category güncellendi" });


                return RedirectToAction("CategoryList");
            }
            return View(model);

        }

        [HttpPost]
        public IActionResult DeletefromCategory(int ProductId, int CategoryId)
        {

            _categoryService.DeletefromCategory(ProductId, CategoryId);
            return Redirect("/admin/categories/" + CategoryId);
        }













        [HttpPost]
        public IActionResult DeleteProduct(int ProductId)
        {
            var entity = _productService.GetById(ProductId);
            if (entity != null)
            {
                _productService.Delete(entity);
            }
            TempData.Put("message", new AlertMessage() { AlertType = "danger", Title = "Ürün Silinmesi", Message = $"{entity.Name} adlı ürün silindi" });


            return RedirectToAction("ProductList");
        }




        [HttpPost]
        public IActionResult DeleteCategory(int Id)
        {
            var entity = _categoryService.GetById(Id);
            if (entity != null)
            {
                _categoryService.Delete(entity);
            }
            TempData.Put("message", new AlertMessage() { AlertType = "danger", Title = "Kategori Silinmesi", Message = $"{entity.Name} kategori ürün silindi" });


            return RedirectToAction("CategoryList");
        }


        public IActionResult RoleList() { return View(_roleManager.Roles); }
        public IActionResult RoleCreate() { return View(); }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {

            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }

            return View();
        }

        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var members = new List<User>();
            var nonmembers = new List<User>();


            foreach (var user in _userManager.Users.ToList())
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonmembers;
                list.Add(user);
            }

            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }


                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
                return Redirect("/admin/role/" + model.RoleId);
            }

            return View();

        }

        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }


        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i => i.Name);
                ViewBag.roles = roles;

                return View(new UserDetailsModel()
                {
                    FirstName = user.FirtsName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles,
                    UserName = user.UserName,
                    UserId = user.Id

                });


            }
            return Redirect("~/admin/user/list");

        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles)
        {

            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;
                    user.FirtsName = model.FirstName;
                    user.LastName = model.LastName;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] { };
                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());
                        return Redirect("/admin/user/list");
                    }




                }

                return Redirect("/admin/user/list");

            }
            return View(model);
        }
    }

}
