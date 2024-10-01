using Microsoft.AspNetCore.Identity;

namespace ECommerceApi.Models
{
    public class User : IdentityUser
    {
        // Propriedades adicionais
        public string Name { get; set; }
        public Cart Cart { get; set; }
    }
}

