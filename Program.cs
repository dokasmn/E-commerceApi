using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

// mideware do projeto
var builder = WebApplication.CreateBuilder(args); // cria um construtor da aplicação

/*
    builder.Services.AddDbContext<EcommerceDb>(opt => opt.UseInMemoryDatabase("Ecommerce")); // adiciona um contexto de banco de dados na memória
    builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // adiciona um filtro de exceção
*/

builder.Services.AddDbContext<EcommerceDb>(options =>
    options.UseMySql("Server=localhost;Database=Ecommerce;User=root;Password=root;", 
    new MySqlServerVersion(new Version(8, 0, 25))));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Adiciona a exploração de endpoints, que é necessária para gerar a documentação da API.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EcommerceAPI", Version = "v1" });
});

//  Constrói a aplicação a partir das configurações definidas.
var app = builder.Build();

// Se o ambiente for de desenvolvimento, habilita o uso do OpenAPI e configura a interface do Swagger.
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

// cria um endpoint get para a aplicação
app.MapGet("/ecommerce", async (EcommerceDb db) =>
    await db.Products.ToListAsync());

// endpoint para inserir um novo produto
app.MapPost("/ecommerce", async (EcommerceDb db, Product product) =>
{
    db.Products.Add(product); // Adiciona o produto ao DbSet
    await db.SaveChangesAsync(); // Salva as alterações no banco de dados em memória
    return Results.Created($"/ecommerce/{product.ProductId}", product); // Retorna o produto criado
});

app.Run();