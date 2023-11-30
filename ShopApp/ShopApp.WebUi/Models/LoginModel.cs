using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUi.Models
{
    public class LoginModel
    {
        [Required]

        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
