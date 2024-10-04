

namespace ECommerceApi.Models
{
    public class CartItem
    {
        public Cart CartItemCart { get; set; }
        public Product CartItemProduct { get; set; }

        public CartItem(Cart CartItemCart, Product CartItemProduct){
            this.CartItemCart = CartItemCart;
            this.CartItemProduct = CartItemProduct;
        }
    }
}



