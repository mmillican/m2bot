using TwitchLib.Client.Interfaces;

namespace M2DevBot.Web.Commands
{
    public class BlogCommand : IChatCommand
    {
        public string Trigger => "blog";
        public string Name => "Blog URL";
        public string Description => "Get the link to Matt's blog";
        
        public void Handle(ITwitchClient twitchClient, string channel, string message, string userName)
        {
            twitchClient.SendMessage(channel, "You can find M2's blog at https://mattmillican.com. PS. Remind him to write more!");
        }
    }
}