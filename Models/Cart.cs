

namespace ECommerceApi.Models
{
    public class Cart
    {
        public User User { get; set; } = new User();
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}



