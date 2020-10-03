using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace M2DevBot.Web.Commands
{
    public class BotCommand : IChatCommand
    {
        public string Trigger => "bot";
        public string Name => "M2 Bot Info";
        public string Description => "Get info about me (the M2 Bot)!";
        public bool ShowInHelp => true;

        public void Handle(ITwitchClient twitchClient, ChatMessage chatMessage)
        {
            twitchClient.SendMessage(chatMessage.Channel,
                @"I am custom built in .NET Core and you can find
                more about me here: https://github.com/mmillican/m2bot");
        }
    }
}
