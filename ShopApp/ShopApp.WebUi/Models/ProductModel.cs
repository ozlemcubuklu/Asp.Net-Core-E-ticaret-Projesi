using ShopApp.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUi.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        //[Display(Name="Name",Prompt ="Enter productName")]
      
        //[Required(ErrorMessage ="Name alanı boş geçilemez.")]  
        //[StringLength(60,MinimumLength =5,ErrorMessage ="name alanı 5-60 karakter arasında olmalı.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Url alanı boş geçilemez.")]
        public string Url { get; set; }
        //[Required(ErrorMessage = "Price alanı boş geçilemez.")]
        //[Range(1,10000,ErrorMessage ="price alanı 1,10000 arasında değer alır.")]
        public double? Price { get; set; }
        [Required(ErrorMessage = "Description alanı boş geçilemez.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "name alanı 5-100 karakter arasında olmalı.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "ImageUrl alanı boş geçilemez.")]
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<Category>  SelectedCategories { get; set; }

    }
}
