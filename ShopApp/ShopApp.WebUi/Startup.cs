using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ShopApp.data.Abstract;
using ShopApp.data;
using ShopApp.data.Concrete.EFCore;
using ShopApp.Entity;
using Microsoft.EntityFrameworkCore;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.WebUi.Identity;
using Microsoft.AspNetCore.Identity;
using ShopApp.WebUi.EmailServices;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace shopapp.webui
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration= configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
           
            // mvc
            // razor pages
            services.AddDbContext<ApplicationContext>(options=>options.UseMySql(_configuration.GetConnectionString("MySqlConnection")));
            services.AddDbContext<ShopContext>(options => options.UseMySql(_configuration.GetConnectionString("MySqlConnection")));
            services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;


                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail=true;

            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;

                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie = new CookieBuilder()
                {
                    
                    HttpOnly=true,
                    Name=".ShopApp.Security.Cookie",
                    SameSite=SameSiteMode.Strict
                };
            });


            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryRepository,EFCoreCategoryRepository>();
            services.AddScoped<ICartService, CartManager>();
            services.AddScoped<IOrderService,OrderManager>();

            services.AddScoped<IProductRepository, ShopApp.data.Concrete.EFCore.EfCoreProductRepository>();
            services.AddScoped<IProductService,ProductManager>();
            services.AddScoped<ICartRepository, EfCoreCartRepository>();
            services.AddScoped<ICategoryRepository,EFCoreCategoryRepository>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IOrderRepository, ShopApp.data.Concrete.EFCore.EfCoreOrderRepository>();
            services.AddControllersWithViews();
            services.AddScoped<IEmailSender, SmtpEmailSender>(i=>new EmailSender(_configuration["EmailSender:Host"],
                _configuration.GetValue<int>("EmailSender:Port"),
                 _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                 _configuration["EmailSender:UserName"],
                 _configuration["EmailSender:Password"]
                )
                );
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IConfiguration configuration,UserManager<User> userManager,RoleManager<IdentityRole> roleManager)
        {
            app.UseStaticFiles(); // wwwroot

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),"node_modules")),
                    RequestPath="/modules"                
            });

            if (env.IsDevelopment())
            {
                SeedDatabase.Seed();
                app.UseDeveloperExceptionPage();
            }
             app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            // localhost:5000
            // localhost:5000/home
            // localhost:5000/home/index
            // localhost:5000/product/details/3
            // localhost:5000/product/list/2
            // localhost:5000/category/list

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
        name: "orders",
        pattern: "orders",
        defaults: new { controller = "Cart", action = "GetOrders" }
        );
                endpoints.MapControllerRoute(
          name: "checkout",
          pattern: "checkout",
          defaults: new { controller = "Cart", action = "Checkout" }
          );
                endpoints.MapControllerRoute(
           name: "cart",
           pattern: "cart",
           defaults: new { controller = "Cart", action = "Index" }
           );

                endpoints.MapControllerRoute(
           name: "adminuseredit",
           pattern: "admin/user/{id?}",
           defaults: new { controller = "Admin", action = "UserEdit" }
           );
                endpoints.MapControllerRoute(
            name: "adminusers",
            pattern: "admin/user/list",
            defaults: new { controller = "Admin", action = "UserList" }
        );

                endpoints.MapControllerRoute(
           name: "adminroleedit",
           pattern: "admin/user/{id?}",
           defaults: new { controller = "Admin", action = "User" });

                endpoints.MapControllerRoute(
            name: "adminroles",
            pattern: "admin/role/list",
            defaults: new { controller = "Admin", action = "RoleList" }
        );
                endpoints.MapControllerRoute(
          name: "adminorders",
          pattern: "admin/order/list",
          defaults: new { controller = "Admin", action = "OrderList" }
      );
                endpoints.MapControllerRoute(
           name: "adminrolecreate",
           pattern: "admin/role/create",
           defaults: new { controller = "Admin", action = "RoleCreate" }
       );
              
                endpoints.MapControllerRoute(
           name: "adminroleedit",
           pattern: "admin/role/{id?}",
           defaults: new { controller = "Admin", action = "RoleEdit" }
       );
                endpoints.MapControllerRoute(
               name: "adminproducts",
               pattern: "admin/products",
               defaults: new { controller = "Admin", action = "ProductList" }
           );
                endpoints.MapControllerRoute(
              name: "adminproductsedit",
              pattern: "admin/products/{id?}",
              defaults: new { controller = "Admin", action = "EditProduct" }
          );
                endpoints.MapControllerRoute(
             name: "adminorderedit",
             pattern: "admin/orders/{id?}",
             defaults: new { controller = "Admin", action = "OrderEdit" }
         );
                endpoints.MapControllerRoute(
              name: "adminproductcreate",
              pattern: "admin/products/create",
              defaults: new { controller = "Admin", action = "CreateProduct" }
          );
                endpoints.MapControllerRoute(
                  name: "admincategories",
                  pattern: "admin/categories",
                  defaults: new { controller = "Admin", action = "CategoryList" }
              );
                endpoints.MapControllerRoute(
                name: "admincategorycreate",
                pattern: "admin/categories/create",
                defaults: new { controller = "Admin", action = "CreateCategory" }
            );
                endpoints.MapControllerRoute(
         name: "adminproductsedit",
         pattern: "admin/categories/{id?}",
         defaults: new { controller = "Admin", action = "EditCategory" }
     );
                endpoints.MapControllerRoute(
                name: "search",
                pattern: "search",
                defaults: new { controller = "Shop", action = "Search" }
            );
                endpoints.MapControllerRoute(
                name: "productsdetails",
                pattern: "{url}",
                defaults: new { controller = "Shop", action = "details" }
            );
                endpoints.MapControllerRoute(
                   name: "products",
                   pattern: "products/{category?}",
                   defaults:new { controller = "Shop", action = "list" }
               );
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern:"{controller=Home}/{action=Index}/{id?}"
                );
            });


            SeedIdentity.Seed(userManager,roleManager,configuration).Wait();
        }
    }

   
}
