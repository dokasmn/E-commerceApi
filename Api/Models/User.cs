using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace ECommerceApi.Models
{
    public class User: IdentityUser 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Column("userName")]
        public string UserName { get; set; }

        [Column("userEmail")]
        public string UserEmail { get; set; }

        [Column("userCart")]
        public Cart UserCart { get; set; }
    }
}
