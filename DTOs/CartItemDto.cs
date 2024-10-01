using ECommerceApi.Models;


namespace ECommerceApi.DTOs
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}