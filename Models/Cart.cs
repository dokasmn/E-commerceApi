

namespace ECommerceApi.Models
{
    public class Cart
    {
        public User CartUser { get; set; } = new User();
        public List<CartItem> CartCartItems { get; set; } = new List<CartItem>();
    }

    // public void AddItem(CartItem item)
    // {
    //     this.CartCartItems.Add(item);
    // }
}



