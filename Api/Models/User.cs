using Microsoft.AspNetCore.Identity;


namespace ECommerceApi.Models
{
    public class User: IdentityUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public Cart UserCart { get; set; }
    }
}
