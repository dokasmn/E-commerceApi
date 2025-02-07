// libs
using Microsoft.AspNetCore.Mvc;
using Supabase;

// project
using ECommerceApi.DTOs;
using ECommerceApi.Models;
using ECommerceApi.Services;
using ECommerceApi.Interfaces;


namespace ECommerceApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
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
                var confirm = await _authService.RegisterAsync(registerModel.Email, registerModel.Password);
                return Ok(new { Message = "Confirmação enviada por e-mail." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Erro interno: " + ex.Message });
            }
        }

    }
}