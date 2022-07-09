using DiscordRPC;
using DiscordRPC.Logging;
using OpenWeatherAPI;
using System;
using System.Threading.Tasks;

namespace Discord_Weather_RPC
{
    internal class Program
    {
        private const string OPEN_WEATHER_API_KEY = "api_key";
        private const string DISCORD_CLIENT_ID = "client_id";
        private const string CITY = "Qiryat Yam";
        private const string RPC_IMAGE_KEY = "weather";

        private static DiscordRpcClient rpcClient;
        private static OpenWeatherApiClient weatherClient;


        private static void init()
        {
            weatherClient = new OpenWeatherApiClient(OPEN_WEATHER_API_KEY);

            rpcClient = new DiscordRpcClient(DISCORD_CLIENT_ID)
            {
                Logger = new ConsoleLogger() { Level = LogLevel.Warning }
            };

            rpcClient.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            rpcClient.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence.GetString());
            };
            rpcClient.Initialize();
        }
        private static async Task Main()
        {
            init();
            await UpdateWeatherAsync(); // start updating
        }

        private static async Task UpdateWeatherAsync()
        {
            while (true)
            {
                QueryResponse response = await weatherClient.QueryAsync(City);

                rpcClient.SetPresence(new RichPresence()
                {
                    Details = City,
                    State = $"{response.Main.Temperature.CelsiusCurrent} °C",
                    Assets = new Assets()
                    {
                        LargeImageKey = "weather",
                        LargeImageText = "Yonka's Weather RPC",
                    }
                });
                await Task.Delay(5000);
            }
        }
    }

    internal static class MExtensions
    {
        internal static string GetString(this BaseRichPresence rpc)
        {
            return $"\n--------\nDetails: {rpc.Details}\nState: {rpc.State}\n--------\n";
        }
    }
}
