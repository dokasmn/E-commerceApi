using System.Text.Json;
using ECommerceApi.Interfaces;

namespace ECommerceApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly Supabase.Client _supabaseClient;


        public AuthService(IConfiguration config)
        {
            var supabaseUrl = config["Supabase:Url"];
            var supabaseKey = config["Supabase:Key"];

            _supabaseClient = new Supabase.Client(supabaseUrl, supabaseKey, new Supabase.SupabaseOptions());
        }


        public async Task<string> RegisterAsync(string email, string password)
        {
            await _supabaseClient.Auth.SignUp(email, password);
            var session = await _supabaseClient.Auth.SignIn(email, password);

            if (session != null)
            {
                return "Usuário criado, mensagem enviada por e-mail"; 
            }

            throw new Exception("Erro ao registrar o usuário.");
        }


        public async Task<string> LoginAsync(string email, string password)
        {
            var session = await _supabaseClient.Auth.SignIn(email, password);

            if (session.User != null && session.AccessToken != null)
            {
                return session.AccessToken;
            }

            throw new Exception("Erro ao fazer login.");
        }


        public async Task LogoutAsync()
        {
            await _supabaseClient.Auth.SignOut();
        }
    }
}
