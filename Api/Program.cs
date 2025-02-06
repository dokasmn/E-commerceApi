using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Identity;


namespace EcommerceApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // desativar em ambiente de produção.
            builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();
            Configure(app);

            Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
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
                new MySqlServerVersion(new Version(8, 0, 25)))
            );

            services.AddDatabaseDeveloperPageExceptionFilter();

            // Configura o Identity, usado para serviços de autenticação
            //ConfigureIdentity(services);

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


        private static void ConfigureIdentity(IServiceCollection services)
        {

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<EcommerceDb>()
                .AddDefaultTokenProviders();


            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
        }


    }
}
