using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace M2DevBot.Web.Commands
{
    public class DiscordCommand : IChatCommand
    {
        public string Trigger => "discord";
        public string Name => "Discord";
        public string Description => "Learn about The Hub Discord Server";
        public bool ShowInHelp => true;

        public void Handle(ITwitchClient twitchClient, ChatMessage chatMessage)
        {
            twitchClient.SendMessage(chatMessage.Channel,
                @"Come join us on 'The Hub' Discord server. The Hub is a group of Sci-Tech Twitch streamers
                with a common goal of sharing knowledge and building each other up. Look for 'The Crew Lounge'
                for our channels, but feel free to join other channels as well. You never know what you'll learn.
                Check it out at https://discord.gg/jw4JRkvDks");
        }
    }
}
