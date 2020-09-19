using TwitchLib.Client.Interfaces;

namespace M2DevBot.Web.Commands
{
    public interface IChatCommand
    {
        string Trigger { get; }

        string Name { get; }
        string Description { get; }
        
        void Handle(ITwitchClient twitchClient, string channel, string message, string userName);
    }
}