using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ECommerceApi.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ProductId { get; set; }

        [Column("productTitle")]
        public string ProductTitle { get; set; }

        [Column("productDescription")]
        public string? ProductDescription { get; set; }

        [Column("productPrice")]
        public double? ProductPrice { get; set; }

        [Column("productThumbnail")]
        public string? ProductThumbnail { get; set; }

        [Column("productIsFeatured")]
        public bool? ProductIsFeatured { get; set; }
    }
}


