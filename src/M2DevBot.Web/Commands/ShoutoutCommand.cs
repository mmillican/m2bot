using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace M2DevBot.Web.Commands
{
    public class ShoutoutCommand : IChatCommand
    {
        public string Trigger => "so";
        public string Name => "Shoutout";

        public string Description => "Shoutout to another streamer";
        public bool ShowInHelp => false;

        public void Handle(ITwitchClient twitchClient, ChatMessage chatMessage)
        {
            if (!chatMessage.IsBroadcaster && !chatMessage.IsModerator)
            {
                return;
            }

            // TODO: Validate name is a valid twitch user
            var name = chatMessage.Message.Replace("!so ", "");
            twitchClient.SendMessage(chatMessage.Channel,
                $"Check out {name}'s stream at https://twitch.tv/{name}");
        }
    }
}
