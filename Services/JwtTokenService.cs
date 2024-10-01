using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

public interface IJwtTokenService
{
    string GenerateToken(IdentityUser user);  // Altere User para IdentityUser
}

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(IdentityUser user)  // Altere User para IdentityUser
    {
        var userId = user.Id;
        var userEmail = user.Email;
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // ID do usuário
            new Claim(JwtRegisteredClaimNames.Email, user.Email),       // Email do usuário
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // ID único do token
        };

        // Obtém a chave secreta da configuração
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Configura o token com claims, tempo de expiração e credenciais de assinatura
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1), // Token expira em 1 hora
            signingCredentials: creds
        );

        // Gera o token como string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
