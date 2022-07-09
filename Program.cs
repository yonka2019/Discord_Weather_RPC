using DiscordRPC;
using DiscordRPC.Logging;
using OpenWeatherAPI;
using System;
using System.Threading.Tasks;

namespace Discord_Weather_RPC
{
    internal class Program
    {
        private static DiscordRpcClient rpcClient;
        private static OpenWeatherApiClient weatherClient;
        private static string weatherIcon;

        private static void Init()
        {
            weatherClient = new OpenWeatherApiClient(Data.OpenWeather.API_KEY);

            rpcClient = new DiscordRpcClient(Data.Discord.CLIENT_ID)
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
            Init();
            await UpdateWeatherAsync(); // start updating
        }

        private static async Task UpdateWeatherAsync()
        {
            while (true)
            {
                QueryResponse response = await weatherClient.QueryAsync(Data.Settings.CITY);
                weatherIcon = response.WeatherList[0].Icon;

                if (weatherIcon.Contains("n")) // night icons are the same as the day icons, so switch night to day
                    weatherIcon = weatherIcon.Replace("n", "d");

                rpcClient.SetPresence(new RichPresence()
                {
                    Details = Data.Settings.CITY,
                    State = $"{response.Main.Temperature.CelsiusCurrent} °C",
                    Assets = new Assets()
                    {
                        LargeImageKey = weatherIcon,
                        LargeImageText = response.WeatherList[0].Description,
                        SmallImageKey = Data.Discord.SMALL_IMAGE_KEY,
                        SmallImageText = "By Yonka"
                    }
                });
                await Task.Delay(Data.Settings.UPDATE_INTERVAL_MS);
            }
        }
    }

    internal static class MExtensions
    {
        internal static string GetString(this BaseRichPresence rpc)
        {
            return $"\n----------\nDetails: {rpc.Details}\nState: {rpc.State}\n----------\n";
        }
    }
}
