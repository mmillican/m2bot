using TwitchLib.Client.Interfaces;

namespace M2DevBot.Web.Commands
{
    public class ProjectCommand : IChatCommand
    {
        public string Trigger => "project";
        public string Name => "Project Info";
        public string Description => "Get information about M2's current project";

        public void Handle(ITwitchClient twitchClient, string channel, string message, string userName)
        {
            // TODO: Pull this from another data source?

            twitchClient.SendMessage(channel, 
                @"M2 is currently working on rebuilding a railroad photo archive app 
                and making this bot more useful.");
        }
    }
}