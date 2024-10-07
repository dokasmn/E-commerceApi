

namespace ECommerceApi.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public Cart CartItemCart { get; set; }
        public Product CartItemProduct { get; set; }

        public CartItem() { }

        public CartItem(Cart CartItemCart, Product CartItemProduct){
            this.CartItemCart = CartItemCart;
            this.CartItemProduct = CartItemProduct;
        }
    }
}



