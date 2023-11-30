using ShopApp.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUi.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name Alanı zorunludur.")]
        [StringLength(100,MinimumLength =5,ErrorMessage ="5-100 arasında karakter içermelidir.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Url Alanı zorunludur.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "5-100 arasında karakter içermelidir.")]
        public string Url { get; set; }
        public List<Product> Products { get; set; }
    }
}
