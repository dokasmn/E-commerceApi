namespace ECommerceApi.Interfaces
{
    public interface IAuthService
    {
        public Task<string> RegisterAsync(string email, string password);
        public Task<string> LoginAsync(string email, string password);
        public Task LogoutAsync();
    }
}