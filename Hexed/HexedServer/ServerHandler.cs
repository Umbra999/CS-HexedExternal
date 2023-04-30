using Hexed.Wrappers;
using Newtonsoft.Json;
using System.Text;

namespace Hexed.HexedServer
{
    internal class ServerHandler
    {
        private static string Token;

        public static async Task Init()
        {
            Logger.Log("Authenticating...");
            if (!File.Exists("Key.Hexed"))
            {
                Logger.LogWarning("Enter Key:");
                string NewKey = Console.ReadLine();
                File.WriteAllText("Key.Hexed", Encryption.ToBase64(NewKey));
            }

            if (!await IsValidKey(Encryption.FromBase64(File.ReadAllText("Key.Hexed"))))
            {
                Logger.LogError("Key is not Valid");
                await Task.Delay(3000);
                Environment.Exit(0);
            }
        }

        private static async Task<string> FetchTime()
        {
            HttpClient Client = new(new HttpClientHandler { UseCookies = false });
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Hexed)");

            HttpRequestMessage Payload = new(HttpMethod.Get, "https://api.logout.rip/Server/Time");
            HttpResponseMessage Response = await Client.SendAsync(Payload);
            if (Response.IsSuccessStatusCode) return await Response.Content.ReadAsStringAsync();
            return null;
        }

        private static async Task<bool> IsValidKey(string Key)
        {
            string Timestamp = await FetchTime();

            HttpClient Client = new(new HttpClientHandler { UseCookies = false });
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Hexed)");

            HttpRequestMessage Payload = new(HttpMethod.Post, "https://api.logout.rip/Server/IsValid")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { Auth = Encryption.EncryptAuthKey(Key, Timestamp, Encryption.GetHWID()) }), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage Response = await Client.SendAsync(Payload);

            if (Response.IsSuccessStatusCode)
            {
                Token = await Response.Content.ReadAsStringAsync();
                return true;
            }

            return false;
        }
    }
}
