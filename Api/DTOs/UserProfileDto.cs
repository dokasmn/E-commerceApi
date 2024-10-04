using ECommerceApi.Models;

namespace ECommerceApi.DTOs
{
    public class UserProfileDto
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public List<Cart> CartList { get; set; }
    }
}