using Microsoft.AspNetCore.Identity;

namespace ECommerceApi.Models
{
    public class User : IdentityUser
    {
        public string UserName { get; set; }
        public Cart UserCart { get; set; }
    }
}

