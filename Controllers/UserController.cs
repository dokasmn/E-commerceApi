using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; // Adicione esta linha
using ECommerceApi.Models; // Supondo que você tenha uma classe Product
using ECommerceApi.DTOs; // Se precisar


namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;


        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            return Ok(new { user.Id, user.UserName, user.Email });
        }



        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDto profileDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            user.UserName = profileDto.UserName;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }


        // [HttpPost("cart")]
        // public async Task<IActionResult> AddToCart([FromBody] CartItemDto cartItemDto)
        // {
        //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //     var user = await _userManager.FindByIdAsync(userId);
        //     if (user == null) return NotFound();
        //     user.Cart.AddItem(cartItemDto.ProductId, cartItemDto.Quantity);
        //     await _userManager.UpdateAsync(user);
        //     return Ok(user.Cart);
        // }
    }
}


