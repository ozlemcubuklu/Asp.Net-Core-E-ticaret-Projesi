namespace ShopApp.WebUi.Models
{
    public class OrderModel
    {
 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public CartModel cartModel { get; set; }
        public string  CartName { get; set; }
        public string CartNumber { get; set; }
        public string  ExpirationMonth { get; set; }
        public string ExpirationYear { get; set;}
        public string Cvc { get; set;}
        public string Note { get; set; }

    }
}
