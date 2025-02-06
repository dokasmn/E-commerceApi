

namespace ECommerceApi.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string CartUserId { get; set; }
        public User CartUser { get; set; }
        public List<CartItem> CartCartItems { get; set; } = new List<CartItem>();
    }

    // public void AddItem(CartItem item)
    // {
    //     this.CartCartItems.Add(item);
    // }
}



