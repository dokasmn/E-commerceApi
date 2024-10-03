using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Middleware do projeto
var builder = WebApplication.CreateBuilder(args);

// Comandos para se conectar com um banco de dados MySQL existente
builder.Services.AddDbContext<EcommerceDb>(options =>
    options.UseMySql("Server=localhost;Database=Ecommerce;User=root;Password=root;",
    new MySqlServerVersion(new Version(8, 0, 25))));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Adiciona serviços de controllers
builder.Services.AddControllers();

// Adiciona configurações de autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

// Adiciona a exploração de endpoints, que é necessária para gerar a documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EcommerceAPI", Version = "v1" });
});

// Constrói a aplicação a partir das configurações definidas
var app = builder.Build();

// Se o ambiente for de desenvolvimento, habilita o uso do OpenAPI e configura a interface do Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EcommerceAPI v1");
        c.DocumentTitle = "EcommerceAPI";
        c.DocExpansion(DocExpansion.List);
    });
}

// Habilita o roteamento dos Controllers da aplicação
app.UseRouting();

// Adiciona autenticação e autorização à aplicação
app.UseAuthentication();
app.UseAuthorization();

// Mapeia os Controllers para o pipeline de requisições
app.MapControllers();

// Inicia a aplicação
app.Run();
