// libs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// project
using ECommerceApi.DTOs;
using ECommerceApi.Models;
using ECommerceApi.Services;


namespace ECommerceApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly AuthService _authService;


        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IJwtTokenService jwtTokenService, AuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthUserDto loginModel)
        {
            try
            {
                var token = await _authService.LoginAsync(loginModel.Email, loginModel.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthUserDto registerModel)
        {
            try
            {
                var token = await _authService.RegisterAsync(registerModel.Email, registerModel.Password);
                Console.WriteLine(string.Join("=",30));
                Console.WriteLine(token);
                Console.WriteLine(string.Join("=",30));
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}