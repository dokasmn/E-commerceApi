using ECommerceApi.Models;


namespace ECommerceApi.DTOs
{
    public class AuthUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
