using TwitchLib.Client.Interfaces;

namespace M2DevBot.Web.Commands
{
    public class EnvironmentCommand : IChatCommand
    {
        public string Trigger => "env";
        public string Name => "Environment Info";
        public string Description => "Get information about M2's environment";

        public void Handle(ITwitchClient twitchClient, string channel, string message, string userName)
        {
            twitchClient.SendMessage(channel, 
                @"M2's primary dev machine is a Windows 10 machine, and he switches 
                between Visual Studio and VS Code, depending what he's working on.");
        }
    }
}