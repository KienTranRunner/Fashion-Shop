using System.ComponentModel.DataAnnotations;

namespace fShop.Models.ViewModels
{
    public class CheckoutViewModel
    {

        public List<CartItem> CartItems { get; set; }  
        public decimal TotalAmount { get; set; }  
        public string Address { get; set; } 
        public string PaymentMethod { get; set; }  
        public User User { get; set; }
    }
}
