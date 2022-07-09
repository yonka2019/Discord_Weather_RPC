namespace Discord_Weather_RPC
{
    internal static class Data
    {
        internal static class Settings
        {
            public const int UPDATE_INTERVAL_MS = 60000; // 60000 / 60 (sec) / 1000 (ms) = 1 minute
            public const string CITY = "";
        }

        internal static class OpenWeather
        {
            public const string API_KEY = "";
        }

        internal static class Discord
        {
            public const string CLIENT_ID = "";
            public const string LARGE_IMAGE_KEY = "";
            public const string SMALL_IMAGE_KEY = "";
        }
    }

}
