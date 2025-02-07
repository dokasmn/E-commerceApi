using Supabase;
using System;
using System.Threading.Tasks;

namespace ECommerceApi.Services
{
    public class AuthService
    {
        private readonly Supabase.Client _supabaseClient;


        public AuthService(IConfiguration config)
        {
            var key = config["key"];
            var url = config["url"];
            _supabaseClient = new Supabase.Client(url, key);
        }


        public async Task<string> RegisterAsync(string email, string password)
        {
            var session = await _supabaseClient.Auth.SignUp(email, password);

            if (session.User != null && session.AccessToken != null)
            {
                return session.AccessToken; 
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
