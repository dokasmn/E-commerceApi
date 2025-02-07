// using Supabase;

// public class SupabaseConnection
// {
//     private Supabase.Client _supabaseClient;

//     public SupabaseConnection(string url, string key)
//     {
//         _supabaseClient = new Supabase.Client(url, key);
//     }

//     public async Task Connect()
//     {
//         try
//         {
//             var session = await _supabaseClient.Auth.SignInWithPasswordAsync("email@example.com", "password");
//             Console.WriteLine("Conectado ao Supabase com sucesso!");
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine("Erro ao conectar ao Supabase: " + ex.Message);
//         }
//     }

//     public Supabase.Client GetClient()
//     {
//         return _supabaseClient;
//     }
// }
