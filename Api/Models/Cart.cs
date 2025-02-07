using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ECommerceApi.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        [Column("cartUser")]
        public User CartUser { get; set; }

        [Column("cartItems")]
        public List<CartItem> CartItems { get; set; }
    }
}
