using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ECommerceApi.Models;
using ECommerceApi.DTOs;


namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly EcommerceDb _context;

        public UserController(UserManager<User> userManager, EcommerceDb context)
        {
            _userManager = userManager;
            _context = context;
        }


        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)return Unauthorized();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            return Ok(new { user.Id, user.UserName, user.Email });
        }



        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDto profileDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)return Unauthorized();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            user.UserName = profileDto.UserName;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }


        [HttpPost("cart")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto cartItemDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)return Unauthorized();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var cart = user.UserCart ?? new Cart { CartUser = user, CartCartItems = new List<CartItem>() };
            var cartProduct = await _context.Products.FindAsync(cartItemDto.ProductId);
            if (cartProduct == null) return NotFound("Produto não encontrado");

            CartItem item = new CartItem(cart, cartProduct);
            cart.CartCartItems.Add(item);
            await _userManager.UpdateAsync(user);
            return Ok(user.UserCart);
        }
    }
}


