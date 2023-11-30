using Microsoft.EntityFrameworkCore;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.data.Concrete.EFCore
{
    public static class SeedDatabase
    {

        public static void Seed()
        {
            //var context=new ShopContext();
            //if (context.Database.GetPendingMigrations().Count()==0)
            //{
            //    if (context.Categories.Count()==0)
            //    {
            //        context.Categories.AddRange(Categories);   
            //    }
            //    if (context.Products.Count() == 0)
            //    {
            //        context.Products.AddRange(Products);
            //        context.AddRange(ProductCategories);
            //    }
            //    context.SaveChanges();


            //}
        }

        private static Category[] Categories = {
                new Category() {Name="Telefon",Url="telefon" },
                new Category() {Name="Bilgisayar",Url="bilgisayar" },
                new Category() {Name="Elektronik" ,Url="elektronik"},
         new Category() {Name="Beyaz Eşya" ,Url="beyaz-esya"}};  
        
        private static Product[] Products = {
         new Product(){Name="Samsung s5",Url="samsung-s5",Description="güzel telefon",ImageUrl="1.jpg",Price=5000,IsApproved=true },
         new Product(){Name="Samsung s6",Url="samsung-s6",Description="güzel telefon",ImageUrl="2.jpg",Price=6000,IsApproved=true },
         new Product(){Name="Samsung s7",Url="samsung-s7",Description="güzel telefon",ImageUrl="3.jpg",Price=57000,IsApproved=false },
         new Product(){Name="Samsung s8",Url="samsung-s8",Description="güzel telefon",ImageUrl="4.jpg",Price=8000,IsApproved=true },
         new Product(){Name="Samsung s9",Url="samsung-s9",Description="güzel telefon",ImageUrl="5.jpg",Price=9000,IsApproved=false }
         };
        private static ProductCategory[] ProductCategories = { 
        
            new ProductCategory(){Product=Products[0],Category=Categories[0] },
            new ProductCategory(){Product=Products[1],Category=Categories[0] },
            new ProductCategory(){Product=Products[2],Category=Categories[0] },
            new ProductCategory(){Product=Products[3],Category=Categories[0] },
            new ProductCategory(){Product=Products[4],Category=Categories[0] },
            new ProductCategory(){Product=Products[0],Category=Categories[2] },
            new ProductCategory(){Product=Products[1],Category=Categories[2] },
            new ProductCategory(){Product=Products[2],Category=Categories[2] },
            new ProductCategory(){Product=Products[3],Category=Categories[2] },
            new ProductCategory(){Product=Products[4],Category=Categories[2] },
        };


     }
    
}
