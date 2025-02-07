using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ECommerceApi.Models
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartItemId { get; set; }


        [ForeignKey("fkCarId")]
        public int CartId { get; set; }

        [ForeignKey("fkProductId")]
        public int ProductId { get; set; }

        [NotMapped]
        public Cart CartItemCart { get; set; }

        [NotMapped]
        public Product CartItemProduct { get; set; }

        public CartItem() { }

        public CartItem(Cart cart, Product product)
        {
            CartItemCart = cart;
            CartItemProduct = product;
        }
    }
}
