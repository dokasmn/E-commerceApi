using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;


namespace EcommerceApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            Configure(app);
            app.Run();
        }


        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Comandos para se conectar com um banco de dados MySQL existente
            services.AddDbContext<EcommerceDb>(options =>
            options.UseMySql(
                @$"Server={configuration["Database:server"]};
                Database={configuration["Database:databaseName"]};
                User={configuration["Database:databaseUser"]};
                Password={configuration["Database:databasePassword"]};",
                new MySqlServerVersion(new Version(8, 0, 25))));

            services.AddDatabaseDeveloperPageExceptionFilter();

            // Adiciona serviços de controllers
            services.AddControllers();

            // Adiciona configurações de autenticação JWT
            ConfigureAuthentication(services, configuration);

            services.AddAuthorization();

            // Adiciona a exploração de endpoints, que é necessária para gerar a documentação da API
            services.AddEndpointsApiExplorer();
            ConfigureSwagger(services);

            // chama a função que gera as políticas (Policy)
            ConfigureAuthorizations(services);
        }


        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });
        }


        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EcommerceAPI", Version = "v1" });
            });
        }


        private static void Configure(WebApplication app)
        {
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
        }

        // exemplo de autorização
        private static void ConfigureAuthorizations(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("validatedSeller", policy =>
                policy.RequireClaim("validated","true"));
            });
        }
    }
}
