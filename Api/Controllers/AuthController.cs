using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerceApi.DTOs;
using ECommerceApi.Models;


namespace ECommerceApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;


        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthUserDto loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (!result.Succeeded) return Unauthorized();

            var token = _jwtTokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthUserDto registerModel)
        {
            Console.WriteLine(string.Join("-",30));
            Console.WriteLine("entrou na função");
            var existingUser = await _userManager.FindByEmailAsync(registerModel.Email);
            if (existingUser != null) return BadRequest("User with this email already exists.");
            try
            {
                var user = new User { UserName = registerModel.Email, Email = registerModel.Email };
                var result = await _userManager.CreateAsync(user, registerModel.Password);
                Console.WriteLine(string.Join("=", 30));
                Console.WriteLine(result);
                Console.WriteLine(string.Join("=", 30));

                if (!result.Succeeded) return BadRequest(result.Errors);
                var token = _jwtTokenService.GenerateToken(user);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Join("+", 30));
                Console.WriteLine("ERRO:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(string.Join("+", 30));
                throw new Exception(ex.Message);
            }
        }
    }
}