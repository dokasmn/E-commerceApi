namespace ECommerceApi.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        // Adicionando as propriedades de chave estrangeira
        public int CartId { get; set; } // Chave estrangeira para Cart
        public int ProductId { get; set; } // Chave estrangeira para Product

        // Propriedades de navegação
        public Cart CartItemCart { get; set; }
        public Product CartItemProduct { get; set; }

        public CartItem() { }

        public CartItem(Cart cart, Product product)
        {
            CartItemCart = cart;
            CartItemProduct = product;
        }
    }
}
