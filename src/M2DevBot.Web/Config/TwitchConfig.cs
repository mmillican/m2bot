namespace M2DevBot.Web.Config
{
    public class TwitchConfig
    {
        public string Username { get; set; }
        public string AccessToken { get; set; }

        public string ChannelName { get; set; }

        /// <summary>
        /// A list of usernames to ignore commands from (mostly bots)
        /// </summary>
        public string[] IgnoredUsernames { get; set; }
    }
}
