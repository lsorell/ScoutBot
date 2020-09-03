using Newtonsoft.Json;

namespace ScoutBot
{
    /// <summary>
    /// Config object for json deserialization.
    /// </summary>
    public class Config
    {
        [JsonProperty("riot_api_key")]
        public static string RiotAPIKey { get; set; }

        [JsonProperty("discord_token")]
        public static string DiscordToken { get; set; }

        /// <summary>
        /// The prefix the discord bot will use to recognize a command.
        /// </summary>
        [JsonProperty("command_prefix")]
        public static char CommandPrefix { get; set; }
    }
}
