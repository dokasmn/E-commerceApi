using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using Supabase;

using ECommerceApi.Models;
using ECommerceApi.Data;
using ECommerceApi.Services;
using ECommerceApi.Interfaces;


namespace EcommerceApi
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // desativar em ambiente de produção.
            builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

            builder.Services.AddDbContext<EcommerceDb>(options =>
            {
                options.UseNpgsql(builder.Configuration["ConnectionStrings:SupabaseConnection"]);
            });

            builder.Services.AddScoped<Supabase.Client>(_ =>
                new Supabase.Client(
                    builder.Configuration["Supabase:Url"],
                    builder.Configuration["Supabase:Key"],
                    new SupabaseOptions
                    {
                        AutoRefreshToken = true,
                        AutoConnectRealtime = true,
                        // SessionHandler = new SupabaseSessionHandler() <-- This must be implemented by the developer
                    }));


            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();
            Configure(app);

            app.Run();
        }


        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IAuthService, AuthService>();

            // Configura o Identity, usado para serviços de autenticação
            ConfigureIdentity(services);

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
            // configurações gerais do Identity
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<EcommerceDb>()
                .AddDefaultTokenProviders();

            // configurando opções do Identity
            services.Configure<IdentityOptions>(options =>
            {
                // opções de senha
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // opções de perda de conta
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // opções de usuário
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            // configuração de cookies
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            // configuração de hash
            services.Configure<PasswordHasherOptions>(option =>
            {
                option.IterationCount = 12000;
            });

            // configuração de 
            services.Configure<SecurityStampValidatorOptions>(o => 
                o.ValidationInterval = TimeSpan.FromMinutes(1));
        }


    }
}
