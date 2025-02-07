namespace ECommerceApi.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public User CartUser { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
