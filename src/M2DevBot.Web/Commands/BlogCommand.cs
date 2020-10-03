using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace M2DevBot.Web.Commands
{
    public class BlogCommand : IChatCommand
    {
        public string Trigger => "blog";
        public string Name => "Blog URL";
        public string Description => "Get the link to Matt's blog";
        public bool ShowInHelp => true;

        public void Handle(ITwitchClient twitchClient, ChatMessage chatMessage)
        {
            twitchClient.SendMessage(chatMessage.Channel, "You can find M2's blog at https://mattmillican.com. PS. Remind him to write more!");
        }
    }
}
