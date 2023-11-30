using Microsoft.AspNetCore.Identity;

namespace ShopApp.WebUi.Identity
{
    public class User:IdentityUser
    {
        public string FirtsName { get; set; }
        public string LastName { get; set; }
    }
}
